using Common.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mobile_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypesController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public UserTypesController(CoffeeShopContext context)
        {
            this._context = context;
        }

        #region Получение данных

        /// <summary>
        /// Получает список всех типов пользователей.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<UserTypes>>> GetAllUserTypes()
        {
            return await this._context.UserTypes
                .OrderBy(t => t.Title)
                .ToListAsync();
        }

        /// <summary>
        /// Получает тип пользователя по его ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTypes>> GetUserTypeById(int id)
        {
            UserTypes? userType = await this._context.UserTypes.FindAsync(id);

            if (userType == null)
            {
                return this.NotFound("Тип пользователя не найден");
            }

            return userType;
        }

        #endregion
    }
}
