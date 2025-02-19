using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class ProductTypes
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Products> Products { get; set; } = new List<Products>();
}
