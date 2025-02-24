using Common.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace REST_API_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsViewController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public IngredientsViewController(CoffeeShopContext context)
        {
            this._context = context;
        }

        // GET: api/IngredientsView
        /// <summary>
        /// Получение всех ингредиентов из представления IngredientsView.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientsView>>> GetIngredientsView()
        {
            try
            {
                // Получаем все элементы из представления IngredientsView
                var ingredients = await this._context.IngredientsView.ToListAsync();

                if (ingredients == null || !ingredients.Any())
                {
                    return this.NotFound("Ингредиенты не найдены.");
                }

                return this.Ok(ingredients);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"Ошибка при получении ингредиентов: {ex.Message}");
            }
        }

        // GET: api/IngredientsView/5
        /// <summary>
        /// Получение ингредиента по ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientsView>> GetIngredientsViewById(int id)
        {
            try
            {
                // Ищем ингредиент по ID в представлении IngredientsView
                var ingredient = await this._context.IngredientsView
                    .FirstOrDefaultAsync(i => i.IngredientId == id);

                if (ingredient == null)
                {
                    return this.NotFound($"Ингредиент с ID {id} не найден.");
                }

                return this.Ok(ingredient);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"Ошибка при получении ингредиента: {ex.Message}");
            }
        }
    }
}
