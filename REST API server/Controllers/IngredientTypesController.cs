using Common.Classes.DB;
using Common.Classes.DTO;
using Microsoft.AspNetCore.Mvc;

namespace REST_API_SERVER.Controllers
{
    /// <summary>
    /// Контроллер для управления типами ингредиентов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientTypesController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="IngredientTypesController"/>.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public IngredientTypesController(CoffeeShopContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Получить список всех типов ингредиентов.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<IngredientTypeDTO>> GetAll()
        {
            var ingredientTypes = this._context.IngredientTypes
                .Select(it => new IngredientTypeDTO
                {
                    Id = it.Id,
                    Title = it.Title
                })
                .ToList();

            return this.Ok(ingredientTypes);
        }

        /// <summary>
        /// Получить тип ингредиента по ID.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<IngredientTypeDTO> GetById(int id)
        {
            IngredientTypeDTO? ingredientType = this._context.IngredientTypes
                .Where(it => it.Id == id)
                .Select(it => new IngredientTypeDTO
                {
                    Id = it.Id,
                    Title = it.Title
                })
                .FirstOrDefault();

            if (ingredientType == null)
            {
                return this.NotFound();
            }

            return this.Ok(ingredientType);
        }

        /// <summary>
        /// Создать новый тип ингредиента.
        /// </summary>
        [HttpPost]
        public ActionResult<IngredientTypeDTO> Create([FromBody] IngredientTypeDTO dto)
        {
            if (dto == null)
            {
                return this.BadRequest();
            }

            var ingredientType = new IngredientTypes
            {
                Title = dto.Title
            };

            _ = this._context.IngredientTypes.Add(ingredientType);
            _ = this._context.SaveChanges();

            dto.Id = ingredientType.Id;

            return this.CreatedAtAction(nameof(GetById), new
            {
                id = dto.Id
            }, dto);
        }

        /// <summary>
        /// Обновить существующий тип ингредиента.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] IngredientTypeDTO dto)
        {
            if (dto == null || id != dto.Id)
            {
                return this.BadRequest();
            }

            IngredientTypes? ingredientType = this._context.IngredientTypes.Find(id);
            if (ingredientType == null)
            {
                return this.NotFound();
            }

            ingredientType.Title = dto.Title;
            _ = this._context.SaveChanges();

            return this.NoContent();
        }

        /// <summary>
        /// Удалить тип ингредиента по ID.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Находим тип ингредиента
            IngredientTypes? ingredientType = this._context.IngredientTypes.Find(id);
            if (ingredientType == null)
            {
                return this.NotFound();
            }

            // Находим все ингредиенты, связанные с этим типом
            var relatedIngredients = this._context.Ingredients
                .Where(i => i.IdIngredientType == id)
                .ToList();

            if (relatedIngredients.Any())
            {
                // Удаляем сначала связанные ингредиенты
                this._context.Ingredients.RemoveRange(relatedIngredients);
            }

            // Удаляем сам тип ингредиента
            _ = this._context.IngredientTypes.Remove(ingredientType);
            _ = this._context.SaveChanges();

            return this.NoContent();
        }

    }
}
