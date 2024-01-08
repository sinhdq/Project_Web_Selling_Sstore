using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ProductDTO
{
    public class ProductListDTO
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = null!;

        public string CategoryName { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }
    }
}
