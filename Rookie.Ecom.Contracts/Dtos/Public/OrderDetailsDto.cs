using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Contracts.Dtos.Public
{
    public class OrderDetailsDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double FinalPrice { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; }
        public int TotalPrice { get; set; }
    }

    public class NewOrderDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string PaymentMethod { get; set; }
        public float Total { get; set; }
        public string UserId { get; set; }
        public string UserAdressId { get; set; }

        public int OrderDetailsId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Rate { get; set; }
        public string Remarks { get; set; }

        public List<OrderDetailsDto> OrderDetailsList { get; set; }
    }
}
