using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.DataAccessor.Models;
using Rookie.Ecom.DataAccessor.Models.Admin;

namespace Rookie.Ecom.DataAccessor.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUsers, ApplicationRoles, string>
    {
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options){
            
        }
        public  DbSet<Brand> Brands { get; set; }
        public  DbSet<Category> Categories { get; set; }
        public  DbSet<Unit> Unit { get; set; }
        public  DbSet<Product> Product { get; set; }
        public  DbSet<ProductComments> ProductComments { get; set; }
        public  DbSet<ProductImage> ProductImage { get; set; }
        public  DbSet<Orders> Order { get; set; }
        public  DbSet<OrderDetails> OrderDetails { get; set; }

        //ModelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Brand>().ToTable("Brand");
            modelBuilder.Entity<Category>().ToTable("Category");

            //Maping tabale
            new BrandMap(modelBuilder.Entity<Brand>());
            new CategoryMap(modelBuilder.Entity<Category>());
            new UnitMap(modelBuilder.Entity<Unit>());
            new ProductMap(modelBuilder.Entity<Product>());
            new ProductCommentsMap(modelBuilder.Entity<ProductComments>());
            new ProductImageMap(modelBuilder.Entity<ProductImage>());

            new OrdersMap(modelBuilder.Entity<Orders>());
            new OrderDetailsMap(modelBuilder.Entity<OrderDetails>());

        }

        




    }
}
