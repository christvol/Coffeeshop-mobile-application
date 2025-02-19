using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class OrderProducts
{
    public int Id { get; set; }

    public int? IdProduct { get; set; }

    public float Total { get; set; }

    public virtual Products? IdProductNavigation { get; set; }

    public virtual ICollection<OrderItemIngredients> OrderItemIngredients { get; set; } = new List<OrderItemIngredients>();

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
}
