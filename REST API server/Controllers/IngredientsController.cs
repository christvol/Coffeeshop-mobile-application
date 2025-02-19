using Common.Classes.DB;
using Common.Classes.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Strings = REST_API_SERVER.Classes.CommonLocal.Strings;

namespace REST_API_SERVER.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public IngredientsController(CoffeeShopContext context)
        {
            this._context = context;
        }

        private bool IngredientExists(int id)
        {
            return this._context.Set<Ingredients>().Any(e => e.Id == id);
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientResponseDto>>> GetIngredients()
        {
            var ingredients = await this._context.Set<Ingredients>()
                .Include(i => i.IdIngredientTypeNavigation)
                .ToListAsync();

            var ingredientDtos = ingredients.Select(i => new IngredientResponseDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Fee = i.Fee,
                IngredientType = i.IdIngredientTypeNavigation != null ? new IngredientTypeDto
                {
                    Id = i.IdIngredientTypeNavigation.Id,
                    Title = i.IdIngredientTypeNavigation.Title
                } : null
            }).ToList();

            return ingredientDtos;
        }

        // GET: api/Ingredients/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientResponseDto>> GetIngredientById(int id)
        {
            var ingredient = await this._context.Set<Ingredients>()
                .Include(i => i.IdIngredientTypeNavigation)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (ingredient == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.IngredientsController.IngredientNotFound
                });
            }

            var ingredientDto = new IngredientResponseDto
            {
                Id = ingredient.Id,
                Title = ingredient.Title,
                Description = ingredient.Description,
                Fee = ingredient.Fee,
                IngredientType = ingredient.IdIngredientTypeNavigation != null ? new IngredientTypeDto
                {
                    Id = ingredient.IdIngredientTypeNavigation.Id,
                    Title = ingredient.IdIngredientTypeNavigation.Title
                } : null
            };

            return ingredientDto;
        }

        // POST: api/Ingredients
        [HttpPost]
        public async Task<ActionResult<IngredientResponseDto>> CreateIngredient([FromBody] IngredientRequestDto ingredientDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);  // Возвращаем ошибку, если модель не валидна
            }

            var ingredient = new Ingredients
            {
                IdIngredientType = ingredientDto.IdIngredientType,
                Title = ingredientDto.Title,
                Description = ingredientDto.Description,
                Fee = ingredientDto.Fee
            };

            _ = this._context.Ingredients.Add(ingredient);
            _ = await this._context.SaveChangesAsync();

            var ingredientResponseDto = new IngredientResponseDto
            {
                Id = ingredient.Id,
                Title = ingredient.Title,
                Description = ingredient.Description,
                Fee = ingredient.Fee,
                IngredientType = ingredient.IdIngredientTypeNavigation != null ? new IngredientTypeDto
                {
                    Id = ingredient.IdIngredientTypeNavigation.Id,
                    Title = ingredient.IdIngredientTypeNavigation.Title
                } : null
            };

            return this.CreatedAtAction(nameof(GetIngredientById), new
            {
                id = ingredient.Id
            }, ingredientResponseDto);
        }


        // PUT: api/Ingredients/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientRequestDto ingredientDto)
        {
            if (id != ingredientDto.Id)
            {
                return this.BadRequest(new
                {
                    Message = Strings.IngredientsController.IngredientIdMismatch
                });
            }

            var existingIngredient = await this._context.Ingredients.FindAsync(id);

            if (existingIngredient == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.IngredientsController.IngredientNotFound
                });
            }

            existingIngredient.IdIngredientType = ingredientDto.IdIngredientType;
            existingIngredient.Title = ingredientDto.Title;
            existingIngredient.Description = ingredientDto.Description;
            existingIngredient.Fee = ingredientDto.Fee;

            try
            {
                _ = await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return this.StatusCode(500, new
                {
                    Message = Strings.IngredientsController.ConcurrencyConflict
                });
            }

            // Возвращаем обновленный объект
            var ingredientResponseDto = new IngredientResponseDto
            {
                Id = existingIngredient.Id,
                Title = existingIngredient.Title,
                Description = existingIngredient.Description,
                Fee = existingIngredient.Fee,
                IngredientType = existingIngredient.IdIngredientTypeNavigation != null ? new IngredientTypeDto
                {
                    Id = existingIngredient.IdIngredientTypeNavigation.Id,
                    Title = existingIngredient.IdIngredientTypeNavigation.Title
                } : null
            };

            return this.Ok(ingredientResponseDto);
        }


        // DELETE: api/Ingredients/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var ingredient = await this._context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.IngredientsController.IngredientNotFound
                });
            }

            _ = this._context.Ingredients.Remove(ingredient);
            _ = await this._context.SaveChangesAsync();

            return this.NoContent();
        }

        // GET: api/Ingredients/{id}/IngredientType
        [HttpGet("{id}/IngredientType")]
        public async Task<ActionResult<IngredientTypeDto>> GetIngredientType(int id)
        {
            var ingredient = await this._context.Set<Ingredients>()
                .Include(i => i.IdIngredientTypeNavigation)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (ingredient == null || ingredient.IdIngredientTypeNavigation == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.IngredientsController.IngredientNotFound
                });
            }

            var ingredientTypeDto = new IngredientTypeDto
            {
                Id = ingredient.IdIngredientTypeNavigation.Id,
                Title = ingredient.IdIngredientTypeNavigation.Title
            };

            return this.Ok(ingredientTypeDto);
        }
    }
}
