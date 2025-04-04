﻿using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class IngredientTypes
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Ingredients> Ingredients { get; set; } = new List<Ingredients>();
}
