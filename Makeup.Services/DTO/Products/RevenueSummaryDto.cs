using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Services.DTO.Products
{
    public class RevenueSummaryDto
    {
        public int ProductId { get; set; }
        public List<RevenueDto>? Revenues { get; set; }
    }
}
