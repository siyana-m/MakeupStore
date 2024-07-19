using Makeup.Services.DTO.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Services.DTO.Orders
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual ProductDto Product { get; set; } = null!;

        public virtual OrderDto Order { get; set; } = null!;
    }
}
