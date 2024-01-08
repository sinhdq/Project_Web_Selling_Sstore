using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Bill
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string? Address { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ShippingDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BillItem> BillItems { get; set; } = new List<BillItem>();

    public virtual Account Customer { get; set; } = null!;
}
