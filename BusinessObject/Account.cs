using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Account
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime? Dob { get; set; }

    public bool? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public bool Active { get; set; }

    public int? Role { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
