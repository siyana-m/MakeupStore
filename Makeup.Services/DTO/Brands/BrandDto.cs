using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Services.DTO.Brands
{
    public class BrandDto
    {
        public int Id { get; set; }
        public byte[]? Logo { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

    }
}
