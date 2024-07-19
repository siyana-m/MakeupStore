using System;
using System.Collections.Generic;

namespace Makeup.Entities;

public partial class Customer
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
