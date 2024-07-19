using Makeup.Services.DTO.Brands;
using Makeup.Services.DTO.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Services.DTO.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte[]? Image { get; set; }
        public string? Code { get; set; }
        public decimal? Price { get; set; }
        public ICollection<BrandDto>? Brands { get; set; }
        public ICollection<CategoryDto>? Categories { get; set; }

    }
}
