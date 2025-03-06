using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class OrderItems
{
    public int Id { get; set; }

    public int IdOrder { get; set; }

    public int IdOrderProduct { get; set; }

    public virtual Orders IdOrderNavigation { get; set; } = null!;

    public virtual OrderProducts IdOrderProductNavigation { get; set; } = null!;
}
