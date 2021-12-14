using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Contracts.Dtos.Public
{
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Quantity { get; set; }
        public double FinalPrice { get; set; }
    }
}
