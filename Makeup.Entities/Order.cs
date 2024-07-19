using System;
using System.Collections.Generic;

namespace Makeup.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDateTime { get; set; }

    public string DeliveryAddress { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public bool IsPaid { get; set; }

    public bool IsDelivered { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
