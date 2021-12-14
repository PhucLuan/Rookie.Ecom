using Rookie.Ecom.Contracts.Dtos.Public;
using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class OrderDetailsAdminDto
    {
        public OrderDetailsAdminDto()
        {
            OrderProductLists = new List<OrderDetailsDto>();
        }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Number { get; set; }
        public string UserAddressId { get; set; }
        public string DeliveryAddress { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string PaymentMethod { get; set; }
        public double Total { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderDetailsDto> OrderProductLists { get; set; }
    }
}
