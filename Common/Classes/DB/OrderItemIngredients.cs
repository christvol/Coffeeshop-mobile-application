using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class OrderItemIngredients
{
    public int Id { get; set; }

    public int IdOrderProduct { get; set; }

    public int? IdIngredient { get; set; }

    public int Amount { get; set; }

    public virtual AllowedIngredients? IdIngredientNavigation { get; set; }

    public virtual OrderProducts IdOrderProductNavigation { get; set; } = null!;
}
