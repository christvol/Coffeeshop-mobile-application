using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class OrderStatuses
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
