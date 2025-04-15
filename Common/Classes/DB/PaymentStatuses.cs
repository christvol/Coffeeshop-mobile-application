using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class PaymentStatuses
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
