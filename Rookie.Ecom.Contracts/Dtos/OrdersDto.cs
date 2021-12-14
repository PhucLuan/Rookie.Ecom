using Rookie.Ecom.DataAccessor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class OrdersDto : BaseEntity
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public double Total { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public string OrderStatus { get; set; }
    }
}
