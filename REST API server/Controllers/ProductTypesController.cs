using Common.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Strings = REST_API_SERVER.Classes.CommonLocal.Strings;

namespace REST_API_SERVER.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTypesController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public ProductTypesController(CoffeeShopContext context)
        {
            this._context = context;
        }

        // GET: api/ProductTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductTypes>>> GetProductTypes()
        {
            return await this._context.ProductTypes.ToListAsync();
        }

        // GET: api/ProductTypes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTypes>> GetProductType(int id)
        {
            var productType = await this._context.ProductTypes.FindAsync(id);

            if (productType == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.ProductTypesController.ProductTypeNotFound
                });
            }

            return productType;
        }

        // POST: api/ProductTypes
        [HttpPost]
        public async Task<ActionResult<ProductTypes>> CreateProductType([FromBody] ProductTypes productType)
        {
            if (string.IsNullOrEmpty(productType.Title))
            {
                return this.BadRequest(new
                {
                    Message = Strings.ProductTypesController.ProductTypeNameRequired
                });
            }

            _ = this._context.ProductTypes.Add(productType);
            _ = await this._context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(GetProductType), new
            {
                id = productType.Id
            }, productType);
        }

        // PUT: api/ProductTypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductType(int id, [FromBody] ProductTypes productType)
        {
            if (id != productType.Id)
            {
                return this.BadRequest(new
                {
                    Message = Strings.ProductTypesController.ProductTypeIdMismatch
                });
            }

            if (string.IsNullOrEmpty(productType.Title))
            {
                return this.BadRequest(new
                {
                    Message = Strings.ProductTypesController.ProductTypeNameRequired
                });
            }

            this._context.Entry(productType).State = EntityState.Modified;

            try
            {
                _ = await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.ProductTypeExists(id))
                {
                    return this.NotFound(new
                    {
                        Message = Strings.ProductTypesController.ProductTypeNotFound
                    });
                }

                throw;
            }

            // Возвращаем обновленный объект в ответе
            return this.Ok(productType);
        }


        // DELETE: api/ProductTypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            var productType = await this._context.ProductTypes.FindAsync(id);

            if (productType == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.ProductTypesController.ProductTypeNotFound
                });
            }

            _ = this._context.ProductTypes.Remove(productType);
            _ = await this._context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool ProductTypeExists(int id)
        {
            return this._context.ProductTypes.Any(e => e.Id == id);
        }
    }
}
