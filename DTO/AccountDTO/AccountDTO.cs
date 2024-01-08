using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.AccountDTO
{
    public class AccountDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime? Dob { get; set; }

        public bool? Gender { get; set; }

        public string? Phone { get; set; }

        public bool Active { get; set; }

        public int? Role { get; set; }
    }
}
