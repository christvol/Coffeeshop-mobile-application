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

        /// <summary>
        /// Удаляет ингредиент из указанного продукта в заказе.
        /// </summary>
        [HttpDelete("{orderId}/product/{productId}")]
        public async Task<IActionResult> DeleteProductFromOrder(int orderId, int productId)
        {
            // Проверяем существование заказа
            Orders? order = await this._context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return this.NotFound("Заказ не найден");
            }

            // Ищем позицию заказа по OrderProductId, а не ProductId!
            OrderDetailsView? orderItemView = await this._context.OrderDetailsView
                .FirstOrDefaultAsync(x => x.OrderId == orderId && x.OrderProductId == productId);

            if (orderItemView == null)
            {
                return this.NotFound("Продукт не найден в заказе");
            }

            // Удаляем соответствующую запись OrderItems
            OrderItems? orderItem = order.OrderItems
                .FirstOrDefault(oi => oi.IdOrderProduct == orderItemView.OrderProductId);

            if (orderItem == null)
            {
                return this.NotFound("Элемент заказа не найден");
            }

            _ = this._context.OrderItems.Remove(orderItem);
            _ = await this._context.SaveChangesAsync();

            return this.NoContent();
        }



        #region Get Order Details

        /// <summary>
        /// Получение всех заказов с деталями.
        /// </summary>
        [HttpGet("details")]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            List<OrderDetailsView> orderDetails = await this._context.OrderDetailsView.ToListAsync();
            return this.Ok(orderDetails);
        }

        /// <summary>
        /// Получение деталей заказа по ID.
        /// </summary>
        [HttpGet("details/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsById(int orderId)
        {
            List<OrderDetailsView> orderDetails = await this._context.OrderDetailsView
                .Where(o => o.OrderId == orderId)
                .ToListAsync();

            if (!orderDetails.Any())
            {
                return this.NotFound("Детали заказа не найдены");
            }

            return this.Ok(orderDetails);
        }

        #endregion

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
                IdStatusPayment = orderDto.IdStatusPayment,
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
                IdStatusPayment = order.IdStatusPayment,
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

        [HttpPut("{id}/payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] int newStatusId)
        {
            Orders? order = await this._context.Orders.FindAsync(id);

            if (order == null)
            {
                return this.NotFound("Заказ не найден");
            }

            order.IdStatusPayment = newStatusId;

            _ = await this._context.SaveChangesAsync();

            return this.Ok("Статус оплаты обновлён");
        }


        [HttpPost("{orderId}/product/{productId}/add-ingredient")]
        public async Task<IActionResult> AddIngredientToOrder(int orderId, int productId, [FromBody] OrderIngredientDTO ingredientDto)
        {
            // Проверяем существование заказа
            Orders? order = await this._context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.IdOrderProductNavigation)
                .ThenInclude(op => op.OrderItemIngredients)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return this.NotFound("Заказ не найден");
            }

            // Проверяем существование продукта в заказе
            OrderProducts? orderProduct = order.OrderItems
                .Select(oi => oi.IdOrderProductNavigation)
                .FirstOrDefault(op => op.IdProduct == productId);

            if (orderProduct == null)
            {
                return this.NotFound("Продукт в заказе не найден");
            }

            // Проверяем существование ингредиента
            Ingredients? ingredient = await this._context.Ingredients.FindAsync(ingredientDto.IdIngredient);
            if (ingredient == null)
            {
                return this.NotFound("Ингредиент не найден");
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


        /// <summary>
        /// Удаляет ингредиент из указанного продукта в заказе.
        /// </summary>
        [HttpDelete("{orderId}/product/{productId}/ingredient/{ingredientId}")]
        public async Task<IActionResult> RemoveIngredientFromOrder(int orderId, int productId, int ingredientId)
        {
            // Находим заказ
            Orders? order = await this._context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.IdOrderProductNavigation)
                .ThenInclude(op => op.OrderItemIngredients)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return this.NotFound("Заказ не найден");
            }

            // Находим нужный продукт в заказе
            OrderProducts? orderProduct = order.OrderItems
                .Select(oi => oi.IdOrderProductNavigation)
                .FirstOrDefault(op => op.IdProduct == productId);

            if (orderProduct == null)
            {
                return this.NotFound("Продукт в заказе не найден");
            }

            // Находим ингредиент
            OrderItemIngredients? ingredient = orderProduct.OrderItemIngredients
                .FirstOrDefault(i => i.IdIngredient == ingredientId);

            if (ingredient == null)
            {
                return this.NotFound("Ингредиент не найден в продукте");
            }

            _ = this._context.OrderItemIngredients.Remove(ingredient);
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

            // Всегда создаём новую позицию — это может быть капучино с ванилью, потом с шоколадом
            var orderProduct = new OrderProducts
            {
                IdProduct = orderProductDto.IdProduct,
                Total = 0 // Цена может пересчитываться позже
            };

            _ = this._context.OrderProducts.Add(orderProduct);
            _ = await this._context.SaveChangesAsync();

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
