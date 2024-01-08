using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Contact
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string Feeback { get; set; } = null!;

    public bool? Status { get; set; }

    public virtual Account Customer { get; set; } = null!;
}
