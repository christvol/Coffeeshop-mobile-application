using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class Orders
{
    public int Id { get; set; }

    public DateTime CreationDate { get; set; }

    public int? IdCustomer { get; set; }

    public int? IdEmployee { get; set; }

    public int IdStatus { get; set; }

    public int IdStatusPayment { get; set; }

    public virtual Users? IdCustomerNavigation { get; set; }

    public virtual Users? IdEmployeeNavigation { get; set; }

    public virtual OrderStatuses IdStatusNavigation { get; set; } = null!;

    public virtual PaymentStatuses IdStatusPaymentNavigation { get; set; } = null!;

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
}
