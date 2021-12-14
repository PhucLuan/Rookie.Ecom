using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rookie.Ecom.DataAccessor.Data;

namespace Rookie.Ecom.DataAccessor.Models.Admin
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Tag { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public Guid UnitId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public bool Ispublish { get; set; }
        public int ProductStock { get; set; }
        public  Category Category { get; set; }
        public  Brand Brand { get; set; }
        public  Unit Unit { get; set; }
        public  ICollection<ProductComments> ProductCommentses { get; set; }
        public  ICollection<ProductImage> ProductImages { get; set; }
        public  ICollection<OrderDetails> OrderDetails { get; set; }
    }

    public class ProductMap
    {
        public ProductMap(EntityTypeBuilder<Product> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Name).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Code).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Tag).HasMaxLength(200);
            entityTypeBuilder.Property(x => x.Description);
            entityTypeBuilder.Property(x => x.Price);
            entityTypeBuilder.Property(x => x.Discount);

            entityTypeBuilder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
            entityTypeBuilder.HasOne(x => x.Brand).WithMany(x => x.Products).HasForeignKey(x => x.BrandId);
            entityTypeBuilder.HasOne(x => x.Unit).WithMany(x => x.Products).HasForeignKey(x => x.UnitId);

        }
    }
}
