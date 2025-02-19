using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class Ingredients
{
    public int Id { get; set; }

    public int? IdIngredientType { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public float Fee { get; set; }

    public virtual ICollection<AllowedIngredients> AllowedIngredients { get; set; } = new List<AllowedIngredients>();

    public virtual IngredientTypes? IdIngredientTypeNavigation { get; set; }

    public virtual ICollection<IngredientImages> IngredientImages { get; set; } = new List<IngredientImages>();
}
