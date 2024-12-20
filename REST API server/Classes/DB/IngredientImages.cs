using System;
using System.Collections.Generic;

namespace REST_API_server.Classes.DB;

public partial class IngredientImages
{
    public byte Id { get; set; }

    public int IdIngredient { get; set; }

    public int IdImage { get; set; }

    public virtual Images IdImageNavigation { get; set; } = null!;

    public virtual Ingredients IdIngredientNavigation { get; set; } = null!;
}
