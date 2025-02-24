using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class IngredientsView
{
    public int IngredientTypeId { get; set; }

    public string IngredientTypeTitle { get; set; } = null!;

    public int IngredientId { get; set; }

    public int? IngredientTypeIdRef { get; set; }

    public string IngredientTitle { get; set; } = null!;

    public string? IngredientDescription { get; set; }

    public float IngredientFee { get; set; }

    public int AllowedIngredientId { get; set; }

    public int AllowedIngredientIdRef { get; set; }

    public int AllowedIngredientProductId { get; set; }

    public byte AllowedIngredientNumber { get; set; }
}
