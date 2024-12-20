using System;
using System.Collections.Generic;

namespace REST_API_server.Classes.DB;

public partial class Users
{
    public int Id { get; set; }

    public int IdUserType { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime BirthDate { get; set; }

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public int? IdImage { get; set; }

    public virtual Images? IdImageNavigation { get; set; }

    public virtual UserTypes IdUserTypeNavigation { get; set; } = null!;

    public virtual ICollection<Orders> OrdersIdCustomerNavigation { get; set; } = new List<Orders>();

    public virtual ICollection<Orders> OrdersIdEmployeeNavigation { get; set; } = new List<Orders>();
}
