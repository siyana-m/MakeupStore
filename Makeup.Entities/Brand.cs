using System;
using System.Collections.Generic;

namespace Makeup.Entities;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? Logo { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
