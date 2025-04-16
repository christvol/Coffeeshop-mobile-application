using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class OrderDetailsView
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public int? CustomerId { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? OrderStatus { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public int OrderItemId { get; set; }

    public int OrderProductId { get; set; }

    public int? ProductId { get; set; }

    public string ProductTitle { get; set; } = null!;

    public float ProductPrice { get; set; }

    public int? OrderItemIngredientId { get; set; }

    public int? IngredientId { get; set; }

    public string? IngredientTitle { get; set; }

    public int? IngredientTypeId { get; set; }

    public float? IngredientFee { get; set; }

    public int? IngredientQuantity { get; set; }

    public double? TotalSum { get; set; }

    public double OrderItemTotal { get; set; }
}
