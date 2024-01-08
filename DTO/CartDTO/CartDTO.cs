using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CartDTO 
{
    public class CartDTO
    {
        public int BillId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }

        public string? Image { get; set; }

        public int Quantity { get; set; }

        public double TotalPrice { get; set; }
    }
}
