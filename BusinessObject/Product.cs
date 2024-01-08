using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public double Price { get; set; }

    public int Quantity { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<BillItem> BillItems { get; set; } = new List<BillItem>();

    public virtual Category Category { get; set; } = null!;
}
