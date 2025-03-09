using Common.Classes.DB;
using Common.Classes.DTO;
using Microsoft.AspNetCore.Mvc;

namespace REST_API_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusesController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public OrderStatusesController(CoffeeShopContext context)
        {
            this._context = context;
        }

        #region Получение всех статусов заказов

        /// <summary>
        /// Получает список всех статусов заказов.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllOrderStatuses()
        {
            var statuses = this._context.OrderStatuses
                .Select(s => new OrderStatusDTO
                {
                    Id = s.Id,
                    Title = s.Title
                })
                .ToList();

            return this.Ok(statuses);
        }

        #endregion

        #region Получение статуса заказа по ID

        /// <summary>
        /// Получает статус заказа по его ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetOrderStatusById(int id)
        {
            OrderStatusDTO? status = this._context.OrderStatuses
                .Where(s => s.Id == id)
                .Select(s => new OrderStatusDTO
                {
                    Id = s.Id,
                    Title = s.Title
                })
                .FirstOrDefault();

            if (status == null)
            {
                return this.NotFound();
            }

            return this.Ok(status);
        }

        #endregion
    }
}
