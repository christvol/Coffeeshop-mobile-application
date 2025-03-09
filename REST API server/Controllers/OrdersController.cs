using Common.Classes.DB;
using Common.Classes.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        #region Create Order

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

        #endregion

        #region Get Orders

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = this._context.Orders
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
                .ToList();

            return this.Ok(orders);
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

        #endregion

        #region Update Order Status

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

        #endregion

        #region Update Order

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDTO orderDto)
        {
            if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            {
                return this.BadRequest("Invalid order data.");
            }

            Orders? order = await this._context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.IdOrderProductNavigation)
                .ThenInclude(op => op.OrderItemIngredients)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return this.NotFound();
            }

            order.IdCustomer = orderDto.IdCustomer;
            order.IdEmployee = orderDto.IdEmployee;
            order.IdStatus = orderDto.IdStatus;

            this._context.OrderItems.RemoveRange(order.OrderItems);

            order.OrderItems = orderDto.OrderItems.Select(oi => new OrderItems
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
            }).ToList();

            _ = await this._context.SaveChangesAsync();

            return this.NoContent();
        }

        #endregion

        #region Modify Order (Adding Products & Ingredients)

        [HttpPost("{orderId}/add-product")]
        public async Task<IActionResult> AddProductToOrder(int orderId, [FromBody] OrderProductDTO orderProductDto)
        {
            Orders? order = await this._context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return this.NotFound("Заказ не найден");
            }

            // Создаем новый продукт для заказа
            var orderProduct = new OrderProducts
            {
                IdProduct = orderProductDto.IdProduct,
                Total = 0, // Цена будет рассчитана позже
            };

            _ = this._context.OrderProducts.Add(orderProduct);
            _ = await this._context.SaveChangesAsync();

            // Добавляем позицию в заказ
            var orderItem = new OrderItems
            {
                IdOrder = orderId,
                IdOrderProduct = orderProduct.Id
            };

            _ = this._context.OrderItems.Add(orderItem);
            _ = await this._context.SaveChangesAsync();

            return this.Ok(new
            {
                orderItem.Id,
                orderItem.IdOrder,
                orderProduct.IdProduct
            });
        }

        [HttpPost("{orderId}/product/{productId}/add-ingredient")]
        public async Task<IActionResult> AddIngredientToProduct(int orderId, int productId, [FromBody] OrderIngredientDTO ingredientDto)
        {
            // Находим позицию заказа
            OrderProducts? orderProduct = await this._context.OrderProducts
                .Include(op => op.OrderItemIngredients)
                .FirstOrDefaultAsync(op => op.IdProduct == productId &&
                                           op.OrderItems.Any(oi => oi.IdOrder == orderId));

            if (orderProduct == null)
            {
                return this.NotFound("Продукт в заказе не найден");
            }

            // Добавляем новый ингредиент
            var orderIngredient = new OrderItemIngredients
            {
                IdOrderProduct = orderProduct.Id,
                IdIngredient = ingredientDto.IdIngredient,
                Amount = ingredientDto.Quantity
            };

            _ = this._context.OrderItemIngredients.Add(orderIngredient);
            _ = await this._context.SaveChangesAsync();

            return this.Ok(new
            {
                orderIngredient.Id,
                orderIngredient.IdOrderProduct,
                orderIngredient.IdIngredient,
                orderIngredient.Amount
            });
        }

        #endregion

        #region Delete Order

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

        #endregion
    }
}
