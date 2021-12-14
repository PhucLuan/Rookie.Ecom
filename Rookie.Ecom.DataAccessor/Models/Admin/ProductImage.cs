using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rookie.Ecom.DataAccessor.Data;

namespace Rookie.Ecom.DataAccessor.Models.Admin
{
    public class ProductImage:BaseEntity
    {
        public Guid ProductId { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string PublicId { get; set; }
        public  Product Product { get; set; }
        public bool Ispublish { get; set; }
        public int Order { get; set; }
    }

    public class ProductImageMap
    {
        public ProductImageMap(EntityTypeBuilder<ProductImage> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.ImagePath);
            entityTypeBuilder.Property(x => x.Title).HasMaxLength(100);

            entityTypeBuilder.HasOne(x => x.Product).WithMany(x => x.ProductImages).HasForeignKey(x => x.ProductId);
        }
    }
}
