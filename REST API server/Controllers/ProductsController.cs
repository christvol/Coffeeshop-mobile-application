using Common.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_API_SERVER.DTOs;

namespace REST_API_SERVER.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public ProductsController(CoffeeShopContext context)
        {
            this._context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await this._context.Products
                .Include(p => p.IdProductTypeNavigation)
                .Include(p => p.ProductImages)
                    .ThenInclude(pi => pi.IdImageNavigation)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Fee = p.Fee,
                    IdProductType = p.IdProductType,
                    ProductTypeTitle = p.IdProductTypeNavigation.Title,
                    ProductImages = p.ProductImages.Select(img => img.IdImageNavigation.Url).ToList()
                })
                .ToListAsync();

            return products;
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await this._context.Products
                .Include(p => p.IdProductTypeNavigation)
                .Include(p => p.ProductImages)
                    .ThenInclude(pi => pi.IdImageNavigation)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Fee = p.Fee,
                    IdProductType = p.IdProductType,
                    ProductTypeTitle = p.IdProductTypeNavigation.Title,
                    ProductImages = p.ProductImages.Select(img => img.IdImageNavigation.Url).ToList()
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return this.NotFound(new
                {
                    Message = "Продукт не найден"
                });
            }

            return product;
        }

        // GET: api/Products/ByType/{typeId}
        [HttpGet("ByType/{typeId}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByType(int typeId)
        {
            var products = await this._context.Products
                .Where(p => p.IdProductType == typeId)
                .Include(p => p.IdProductTypeNavigation)
                .Include(p => p.ProductImages)
                    .ThenInclude(pi => pi.IdImageNavigation)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Fee = p.Fee,
                    IdProductType = p.IdProductType,
                    ProductTypeTitle = p.IdProductTypeNavigation.Title,
                    ProductImages = p.ProductImages.Select(img => img.IdImageNavigation.Url).ToList()
                })
                .ToListAsync();

            return products;
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductDTO productDTO)
        {
            if (string.IsNullOrEmpty(productDTO.Title))
            {
                return this.BadRequest(new
                {
                    Message = "Название продукта обязательно"
                });
            }

            var productType = await this._context.ProductTypes.FindAsync(productDTO.IdProductType);
            if (productType == null)
            {
                return this.BadRequest(new
                {
                    Message = "Указанный тип продукта не существует"
                });
            }

            // Преобразуем DTO в сущность Products для сохранения в БД
            var product = new Products
            {
                Title = productDTO.Title,
                Description = productDTO.Description,
                Fee = productDTO.Fee,
                IdProductType = productDTO.IdProductType
            };

            _ = this._context.Products.Add(product);
            _ = await this._context.SaveChangesAsync();

            // Формируем DTO для возврата
            var createdProductDTO = new ProductDTO
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Fee = product.Fee,
                IdProductType = product.IdProductType,
                ProductTypeTitle = productType.Title,
                ProductImages = new List<string>() // Пока изображений нет, создаем пустой список
            };

            return this.CreatedAtAction(nameof(GetProduct), new
            {
                id = product.Id
            }, createdProductDTO);
        }



        // PUT: api/Products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Products product)
        {
            if (id != product.Id)
            {
                return this.BadRequest(new
                {
                    Message = "ID продукта не совпадает"
                });
            }

            if (string.IsNullOrEmpty(product.Title))
            {
                return this.BadRequest(new
                {
                    Message = "Название продукта обязательно"
                });
            }

            var productType = await this._context.ProductTypes.FindAsync(product.IdProductType);
            if (productType == null)
            {
                return this.BadRequest(new
                {
                    Message = "Указанный тип продукта не существует"
                });
            }

            this._context.Entry(product).State = EntityState.Modified;

            try
            {
                _ = await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.ProductExists(id))
                {
                    return this.NotFound(new
                    {
                        Message = "Продукт не найден"
                    });
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }

        // DELETE: api/Products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await this._context.Products.FindAsync(id);
            if (product == null)
            {
                return this.NotFound(new
                {
                    Message = "Продукт не найден"
                });
            }

            _ = this._context.Products.Remove(product);
            _ = await this._context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool ProductExists(int id)
        {
            return this._context.Products.Any(p => p.Id == id);
        }
    }
}
