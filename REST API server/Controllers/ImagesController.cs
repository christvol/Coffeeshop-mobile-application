using Common.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace REST_API_SERVER.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public ImagesController(CoffeeShopContext context)
        {
            this._context = context;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Images>>> GetAllImages()
        {
            return await this._context.Images.ToListAsync();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Images>> GetImage(int id)
        {
            Images? image = await this._context.Images.FindAsync(id);

            if (image == null)
            {
                return this.NotFound("Изображение не найдено");
            }

            return image;
        }

        // GET: api/Images/5/data
        [HttpGet("{id}/data")]
        public async Task<IActionResult> GetImageData(int id)
        {
            Images? image = await this._context.Images.FindAsync(id);
            if (image == null || image.Data == null)
            {
                return this.NotFound("Изображение не найдено или не содержит данных");
            }

            return this.File(image.Data, "image/png");
        }

        // POST: api/Images
        [HttpPost]
        public async Task<ActionResult<Images>> CreateImage([FromBody] Images image)
        {
            if (image == null)
            {
                return this.BadRequest("Изображение не передано");
            }

            _ = this._context.Images.Add(image);
            _ = await this._context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(GetImage), new
            {
                id = image.Id
            }, image);
        }

        // POST: api/Images/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest("Файл не передан");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var image = new Images
            {
                Title = file.FileName,
                Description = "Загружено через upload",
                Url = $"/images/{file.FileName}",
                Data = memoryStream.ToArray()
            };

            _ = this._context.Images.Add(image);
            _ = await this._context.SaveChangesAsync();

            return this.Ok(new
            {
                image.Id
            });
        }

        // PUT: api/Images/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(int id, [FromBody] Images image)
        {
            if (id != image.Id)
            {
                return this.BadRequest("ID изображения не совпадает");
            }

            Images? existingImage = await this._context.Images.FindAsync(id);
            if (existingImage == null)
            {
                return this.NotFound("Изображение не найдено");
            }

            existingImage.Title = image.Title;
            existingImage.Description = image.Description;
            existingImage.Url = image.Url;
            existingImage.Data = image.Data;

            this._context.Entry(existingImage).State = EntityState.Modified;
            _ = await this._context.SaveChangesAsync();

            return this.NoContent();
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            Images? image = await this._context.Images.FindAsync(id);
            if (image == null)
            {
                return this.NotFound("Изображение не найдено");
            }

            _ = this._context.Images.Remove(image);
            _ = await this._context.SaveChangesAsync();

            return this.NoContent();
        }
    }
}
