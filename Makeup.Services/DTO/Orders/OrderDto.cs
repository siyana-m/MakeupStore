using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Services.DTO.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public bool IsPaid { get; set; }
        public bool IsDelivered { get; set; }

        public virtual CustomerDto Customer { get; set; } = null!;
        public virtual ICollection<OrderDetailsDto>? OrderDetails { get; set; }
    }
}
