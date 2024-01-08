using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class BillItem
{
    public int BillId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public double TotalPrice { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
