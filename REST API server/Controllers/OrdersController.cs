using Common.Classes.DB;
using Microsoft.AspNetCore.Mvc;

namespace REST_API_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public OrdersController(CoffeeShopContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDto)
        {
            if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            {
                return this.BadRequest("Invalid order data.");
            }

            var order = new Orders
            {
                CreationDate = DateTime.UtcNow,
                IdCustomer = orderDto.IdCustomer,
                IdEmployee = orderDto.IdEmployee,
                IdStatus = orderDto.IdStatus,
                OrderItems = orderDto.OrderItems.Select(oi => new OrderItems
                {
                    IdOrderProductNavigation = new OrderProducts
                    {
                        IdProduct = oi.IdProduct,
                        Total = oi.Total,
                        OrderItemIngredients = oi.Ingredients?.Select(ing => new OrderItemIngredients
                        {
                            IdIngredient = ing.IdIngredient,
                            Amount = ing.Amount
                        }).ToList() ?? new List<OrderItemIngredients>()
                    }
                }).ToList()
            };

            _ = this._context.Orders.Add(order);
            _ = await this._context.SaveChangesAsync();

            var orderDtoResponse = new OrderDTO
            {
                Id = order.Id,
                IdCustomer = order.IdCustomer,
                IdEmployee = order.IdEmployee,
                IdStatus = order.IdStatus,
                OrderItems = order.OrderItems.Select(oi => new OrderItemsDTO
                {
                    IdProduct = oi.IdOrderProductNavigation.IdProduct,
                    Total = oi.IdOrderProductNavigation.Total,
                    Ingredients = oi.IdOrderProductNavigation.OrderItemIngredients?.Select(i => new OrderItemIngredientDTO
                    {
                        IdIngredient = i.IdIngredient,
                        Amount = i.Amount
                    }).ToList() ?? new List<OrderItemIngredientDTO>()
                }).ToList()
            };

            return this.CreatedAtAction(nameof(GetOrder), new
            {
                id = order.Id
            }, orderDtoResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = this._context.Orders
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    o.CreationDate,
                    o.IdCustomer,
                    o.IdEmployee,
                    o.IdStatus,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemsDTO
                    {
                        IdProduct = oi.IdOrderProductNavigation.IdProduct,
                        Total = oi.IdOrderProductNavigation.Total,
                        Ingredients = oi.IdOrderProductNavigation.OrderItemIngredients.Select(i => new OrderItemIngredientDTO
                        {
                            IdIngredient = i.IdIngredient,
                            Amount = i.Amount
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefault();

            if (order == null)
            {
                return this.NotFound();
            }

            return this.Ok(order);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] int newStatus)
        {
            Orders? order = await this._context.Orders.FindAsync(id);
            if (order == null)
            {
                return this.NotFound();
            }

            order.IdStatus = newStatus;
            _ = await this._context.SaveChangesAsync();

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Orders? order = await this._context.Orders.FindAsync(id);
            if (order == null)
            {
                return this.NotFound();
            }

            _ = this._context.Orders.Remove(order);
            _ = await this._context.SaveChangesAsync();

            return this.NoContent();
        }
    }

    public class OrderDTO
    {
        public int Id
        {
            get; set;
        }
        public int? IdCustomer
        {
            get; set;
        }
        public int? IdEmployee
        {
            get; set;
        }
        public int IdStatus
        {
            get; set;
        }
        public List<OrderItemsDTO> OrderItems
        {
            get; set;
        }
    }

    public class OrderItemsDTO
    {
        public int? IdProduct
        {
            get; set;
        }
        public float Total
        {
            get; set;
        }
        public List<OrderItemIngredientDTO> Ingredients
        {
            get; set;
        }
    }

    public class OrderItemIngredientDTO
    {
        public int? IdIngredient
        {
            get; set;
        }
        public int Amount
        {
            get; set;
        }
    }
}
