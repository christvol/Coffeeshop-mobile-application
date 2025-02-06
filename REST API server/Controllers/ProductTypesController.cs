using Common.Classes.DB;
using Common.Classes.DTO;
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
        public async Task<ActionResult<IEnumerable<ProductTypeDTO>>> GetProductTypes()
        {
            var productTypes = await this._context.ProductTypes
                .Select(pt => new ProductTypeDTO
                {
                    Id = pt.Id,
                    Title = pt.Title
                })
                .ToListAsync();

            return this.Ok(productTypes);
        }

        // GET: api/ProductTypes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTypeDTO>> GetProductType(int id)
        {
            var productType = await this._context.ProductTypes
                .Where(pt => pt.Id == id)
                .Select(pt => new ProductTypeDTO
                {
                    Id = pt.Id,
                    Title = pt.Title
                })
                .FirstOrDefaultAsync();

            if (productType == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.ProductTypesController.ProductTypeNotFound
                });
            }

            return this.Ok(productType);
        }

        // POST: api/ProductTypes
        [HttpPost]
        public async Task<ActionResult<ProductTypeDTO>> CreateProductType([FromBody] ProductTypeDTO productTypeDto)
        {
            if (string.IsNullOrEmpty(productTypeDto.Title))
            {
                return this.BadRequest(new
                {
                    Message = Strings.ProductTypesController.ProductTypeNameRequired
                });
            }

            var productType = new ProductTypes
            {
                Title = productTypeDto.Title
            };

            _ = this._context.ProductTypes.Add(productType);
            _ = await this._context.SaveChangesAsync();

            productTypeDto.Id = productType.Id;

            return this.CreatedAtAction(nameof(GetProductType), new
            {
                id = productTypeDto.Id
            }, productTypeDto);
        }

        // PUT: api/ProductTypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductType(int id, [FromBody] ProductTypeDTO productTypeDto)
        {
            if (id != productTypeDto.Id)
            {
                return this.BadRequest(new
                {
                    Message = Strings.ProductTypesController.ProductTypeIdMismatch
                });
            }

            if (string.IsNullOrEmpty(productTypeDto.Title))
            {
                return this.BadRequest(new
                {
                    Message = Strings.ProductTypesController.ProductTypeNameRequired
                });
            }

            var productType = await this._context.ProductTypes.FindAsync(id);

            if (productType == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.ProductTypesController.ProductTypeNotFound
                });
            }

            productType.Title = productTypeDto.Title;

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

            return this.Ok(productTypeDto);
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
