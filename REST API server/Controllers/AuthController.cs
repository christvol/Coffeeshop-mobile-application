using DB.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace REST_API_SERVER.Controllers
{
    public class RegistrationRequest
    {
        public string PhoneNumber
        {
            get; set;
        }
    }

    public class VerificationRequest
    {
        public string PhoneNumber
        {
            get; set;
        }
        public string Code
        {
            get; set;
        }
        public int UserTypeId
        {
            get; set;
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, (string Code, DateTime Expiry)> VerificationCodes = new();
        private readonly CoffeeShopContext _context;

        public AuthController(CoffeeShopContext context)
        {
            this._context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PhoneNumber))
                {
                    return this.BadRequest(new
                    {
                        Message = "Номер телефона обязателен."
                    });
                }

                // Удаление всех символов, кроме цифр и знака '+'
                string cleanedPhoneNumber = new string(request.PhoneNumber.Where(c => char.IsDigit(c) || c == '+').ToArray());

                // Проверка формата номера телефона: начинается с '+', за которым следует от 11 до 20 цифр
                if (!Regex.IsMatch(cleanedPhoneNumber, @"^\+\d{11,20}$"))
                {
                    return this.BadRequest(new
                    {
                        Message = "Неверный формат номера телефона. Ожидается формат: + и от 11 до 20 цифр."
                    });
                }

                // Проверка существования пользователя
                var existingUser = this._context.Users.FirstOrDefault(u => u.PhoneNumber == cleanedPhoneNumber);
                if (existingUser != null)
                {
                    var authToken = this.GenerateAuthToken();
                    return this.Ok(new
                    {
                        UserId = existingUser.Id,
                        AuthToken = authToken
                    });
                }

                // Генерация 4-значного кода
                var code = new Random().Next(1000, 9999).ToString();
                var expiry = DateTime.UtcNow.AddSeconds(30);
                VerificationCodes[cleanedPhoneNumber] = (code, expiry);

                return this.Ok(new
                {
                    Message = "Код подтверждения отправлен.",
                    Code = code
                });
            }
            catch (Exception)
            {
                // Логирование ошибки (при необходимости)
                // _logger.LogError(ex, "Ошибка в методе Register");

                return this.StatusCode(500, new
                {
                    Message = "Внутренняя ошибка сервера. Пожалуйста, попробуйте позже."
                });
            }
        }


        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerificationRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Code))
            {
                return this.BadRequest(new
                {
                    Message = "Номер телефона и код обязательны."
                });
            }

            if (VerificationCodes.TryGetValue(request.PhoneNumber, out var codeInfo))
            {
                if (codeInfo.Expiry < DateTime.UtcNow)
                {
                    VerificationCodes.TryRemove(request.PhoneNumber, out _);
                    return this.BadRequest(new
                    {
                        Message = "Код истек. Пожалуйста, запросите новый."
                    });
                }

                if (codeInfo.Code == request.Code)
                {
                    var newUser = new Users
                    {
                        PhoneNumber = new string(request.PhoneNumber.Where(char.IsDigit).ToArray()),
                        BirthDate = DateTime.UtcNow,
                        IdUserType = request.UserTypeId
                    };

                    this._context.Users.Add(newUser);
                    await this._context.SaveChangesAsync();

                    var authToken = this.GenerateAuthToken();
                    VerificationCodes.TryRemove(request.PhoneNumber, out _);

                    return this.Ok(new
                    {
                        UserId = newUser.Id,
                        AuthToken = authToken
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        Message = "Неверный код."
                    });
                }
            }
            else
            {
                return this.BadRequest(new
                {
                    Message = "Код не найден. Пожалуйста, запросите новый."
                });
            }
        }

        private string GenerateAuthToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
