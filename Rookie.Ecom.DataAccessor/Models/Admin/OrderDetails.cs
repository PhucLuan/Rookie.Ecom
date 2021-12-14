using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Rookie.Ecom.DataAccessor.Data;

namespace Rookie.Ecom.DataAccessor.Models.Admin
{
    public class OrderDetails :BaseEntity
    {
        
        public Guid OrderId { get; set; }
        
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public string Remarks { get; set; }

        public  Orders Orders { get; set; }
        public  Product Product { get; set; }
    }

    public class OrderDetailsMap
    {
        public OrderDetailsMap(EntityTypeBuilder<OrderDetails> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Quantity);
            entityTypeBuilder.Property(x => x.Rate);
            entityTypeBuilder.Property(x => x.Remarks).HasMaxLength(200);
            entityTypeBuilder.HasOne(x => x.Product).WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.ProductId);
            entityTypeBuilder.HasOne(x => x.Orders).WithMany(x => x.OrderDetails)
                .HasForeignKey(k=>k.OrderId);
        }
    }
}
//(localdb)\\mssqllocaldb