using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class AllowedIngredients
{
    public int Id { get; set; }

    public int IdIngredient { get; set; }

    public int IdProduct { get; set; }

    public byte AllowedNumber { get; set; }

    public virtual Ingredients IdIngredientNavigation { get; set; } = null!;

    public virtual Products IdProductNavigation { get; set; } = null!;
}
