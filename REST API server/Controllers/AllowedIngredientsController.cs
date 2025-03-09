using Common.Classes.DB;
using Common.Classes.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowedIngredientsController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public AllowedIngredientsController(CoffeeShopContext context)
        {
            this._context = context;
        }

        // GET: api/AllowedIngredients
        [HttpGet]
        public ActionResult<IEnumerable<AllowedIngredientsDTO>> GetAllowedIngredients()
        {
            var allowedIngredients = this._context.AllowedIngredients
                .Include(ai => ai.OrderItemIngredients)
                .Select(ai => new AllowedIngredientsDTO
                {
                    Id = ai.Id,
                    IdIngredient = ai.IdIngredient,
                    IdProduct = ai.IdProduct,
                    AllowedNumber = ai.AllowedNumber,
                    OrderItemIngredients = ai.OrderItemIngredients.Select(oii => new OrderItemIngredientDTO
                    {
                        Id = oii.Id,
                        IdOrderProduct = oii.IdOrderProduct,
                        IdIngredient = oii.IdIngredient ?? 0, // Можно добавить проверку на null
                        Amount = oii.Amount
                    }).ToList()
                }).ToList();

            return this.Ok(allowedIngredients);
        }

        // GET: api/AllowedIngredients/5
        [HttpGet("{id}")]
        public ActionResult<AllowedIngredientsDTO> GetAllowedIngredientsById(int id)
        {
            AllowedIngredients? allowedIngredient = this._context.AllowedIngredients
                .Include(ai => ai.OrderItemIngredients)
                .FirstOrDefault(ai => ai.Id == id);

            if (allowedIngredient == null)
            {
                return this.NotFound();
            }

            var allowedIngredientsDTO = new AllowedIngredientsDTO
            {
                Id = allowedIngredient.Id,
                IdIngredient = allowedIngredient.IdIngredient,
                IdProduct = allowedIngredient.IdProduct,
                AllowedNumber = allowedIngredient.AllowedNumber,
                OrderItemIngredients = allowedIngredient.OrderItemIngredients.Select(oii => new OrderItemIngredientDTO
                {
                    Id = oii.Id,
                    IdOrderProduct = oii.IdOrderProduct,
                    IdIngredient = oii.IdIngredient ?? 0, // Можно добавить проверку на null
                    Amount = oii.Amount
                }).ToList()
            };

            return this.Ok(allowedIngredientsDTO);
        }

        // POST: api/AllowedIngredients
        [HttpPost]
        public ActionResult<AllowedIngredientsDTO> CreateAllowedIngredient(AllowedIngredientsDTO allowedIngredientsDTO)
        {
            var allowedIngredient = new AllowedIngredients
            {
                IdIngredient = allowedIngredientsDTO.IdIngredient,
                IdProduct = allowedIngredientsDTO.IdProduct,
                AllowedNumber = allowedIngredientsDTO.AllowedNumber
            };

            _ = this._context.AllowedIngredients.Add(allowedIngredient);
            _ = this._context.SaveChanges();

            allowedIngredientsDTO.Id = allowedIngredient.Id;

            return this.CreatedAtAction(nameof(GetAllowedIngredientsById), new
            {
                id = allowedIngredient.Id
            }, allowedIngredientsDTO);
        }

        // PUT: api/AllowedIngredients/5
        [HttpPut("{id}")]
        public IActionResult UpdateAllowedIngredient(int id, AllowedIngredientsDTO allowedIngredientsDTO)
        {
            if (id != allowedIngredientsDTO.Id)
            {
                return this.BadRequest();
            }

            AllowedIngredients? allowedIngredient = this._context.AllowedIngredients.Find(id);

            if (allowedIngredient == null)
            {
                return this.NotFound();
            }

            allowedIngredient.IdIngredient = allowedIngredientsDTO.IdIngredient;
            allowedIngredient.IdProduct = allowedIngredientsDTO.IdProduct;
            allowedIngredient.AllowedNumber = allowedIngredientsDTO.AllowedNumber;

            this._context.Entry(allowedIngredient).State = EntityState.Modified;
            _ = this._context.SaveChanges();

            // Возвращаем обновлённый объект
            var updatedAllowedIngredientDTO = new AllowedIngredientsDTO
            {
                Id = allowedIngredient.Id,
                IdIngredient = allowedIngredient.IdIngredient,
                IdProduct = allowedIngredient.IdProduct,
                AllowedNumber = allowedIngredient.AllowedNumber,
                // Преобразуйте другие необходимые свойства, если нужно
            };

            return this.Ok(updatedAllowedIngredientDTO);
        }


        // DELETE: api/AllowedIngredients/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAllowedIngredient(int id)
        {
            AllowedIngredients? allowedIngredient = this._context.AllowedIngredients.Find(id);

            if (allowedIngredient == null)
            {
                return this.NotFound();
            }

            _ = this._context.AllowedIngredients.Remove(allowedIngredient);
            _ = this._context.SaveChanges();

            return this.NoContent();
        }
    }
}
