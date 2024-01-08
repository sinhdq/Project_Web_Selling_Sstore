using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.BillDTO
{
    public class BillDTO
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string? Address { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? ShippingDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public bool? Status { get; set; }
    }
}
