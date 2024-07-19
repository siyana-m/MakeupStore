using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Services.DTO.Products
{
    public class CreateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public decimal? Price { get; set; }
        public List<int>? Brands { get; set; }
        public List<int>? Categories { get; set; }
    }

}
