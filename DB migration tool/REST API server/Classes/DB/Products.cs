using System;
using System.Collections.Generic;

namespace DB.Classes.DB;

public partial class Products
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public float Fee { get; set; }

    public int IdProductType { get; set; }

    public virtual ICollection<AllowedIngredients> AllowedIngredients { get; set; } = new List<AllowedIngredients>();

    public virtual ProductTypes IdProductTypeNavigation { get; set; } = null!;

    public virtual ICollection<OrderProducts> OrderProducts { get; set; } = new List<OrderProducts>();

    public virtual ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();
}
