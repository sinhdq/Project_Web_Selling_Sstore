using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ContactDTO
{
    public class ContactManagerDTO
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string Feeback { get; set; } = null!;

        public bool? Status { get; set; }

    }
}
