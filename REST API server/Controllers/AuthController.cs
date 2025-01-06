using Common.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using REST_API_SERVER.Classes.Requests;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Strings = REST_API_SERVER.Classes.CommonLocal.Strings;

namespace REST_API_SERVER.Controllers
{
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
                        Message = Strings.ErrorMessages.PhoneNumberRequired
                    });
                }

                string cleanedPhoneNumber = new string(request.PhoneNumber.Where(c => char.IsDigit(c) || c == '+').ToArray());

                if (!Regex.IsMatch(cleanedPhoneNumber, Strings.ValidationPatterns.PhoneNumber))
                {
                    return this.BadRequest(new
                    {
                        Message = Strings.ErrorMessages.InvalidPhoneNumberFormat
                    });
                }

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

                var code = new Random().Next(1000, 9999).ToString();
                var expiry = DateTime.UtcNow.AddSeconds(30);
                VerificationCodes[cleanedPhoneNumber] = (code, expiry);

                return this.Ok(new
                {
                    Message = Strings.SuccessMessages.VerificationCodeSent,
                    Code = code
                });
            }
            catch (Exception)
            {
                return this.StatusCode(500, new
                {
                    Message = Strings.ErrorMessages.ServerError
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
                    Message = Strings.ErrorMessages.PhoneNumberRequired
                });
            }

            if (VerificationCodes.TryGetValue(request.PhoneNumber, out var codeInfo))
            {
                if (codeInfo.Expiry < DateTime.UtcNow)
                {
                    VerificationCodes.TryRemove(request.PhoneNumber, out _);
                    return this.BadRequest(new
                    {
                        Message = Strings.ErrorMessages.VerificationCodeExpired
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
                        Message = Strings.ErrorMessages.InvalidVerificationCode
                    });
                }
            }
            else
            {
                return this.BadRequest(new
                {
                    Message = Strings.ErrorMessages.VerificationCodeNotFound
                });
            }
        }

        private string GenerateAuthToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
