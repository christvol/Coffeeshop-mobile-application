using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class Orders
{
    public int Id { get; set; }

    public DateTime CreationDate { get; set; }

    public int? IdCustomer { get; set; }

    public int? IdEmployee { get; set; }

    public virtual Users? IdCustomerNavigation { get; set; }

    public virtual Users? IdEmployeeNavigation { get; set; }

    public virtual ICollection<OrderIngredients> OrderIngredients { get; set; } = new List<OrderIngredients>();

    public virtual ICollection<OrderProducts> OrderProducts { get; set; } = new List<OrderProducts>();
}
