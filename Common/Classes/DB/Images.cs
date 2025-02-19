using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class Images
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string Url { get; set; } = null!;

    public byte[]? Data { get; set; }

    public virtual ICollection<IngredientImages> IngredientImages { get; set; } = new List<IngredientImages>();

    public virtual ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
