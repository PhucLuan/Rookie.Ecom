using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rookie.Ecom.DataAccessor.Data;

namespace Rookie.Ecom.DataAccessor.Models.Admin
{
    public class Orders : BaseEntity
    {
        public string Number { get; set; }
        public string UserId { get; set; }
        public double Total { get; set; }
        public double OthersCharge { get; set; }
        public string PaymentMethod { get; set; }
        public string UserAdressId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public  ICollection<OrderDetails> OrderDetails { get; set; }        
    }

    public class OrdersMap
    {
        public OrdersMap(EntityTypeBuilder<Orders> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Number).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Total);
            entityTypeBuilder.Property(x => x.OthersCharge);
        }
    }
}
