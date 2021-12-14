using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rookie.Ecom.DataAccessor.Data;

namespace Rookie.Ecom.DataAccessor.Models.Admin
{
    public class Brand:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }

    public class BrandMap
    {
        public BrandMap(EntityTypeBuilder<Brand> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(b => b.Id);
            entityTypeBuilder.Property(b => b.Name);
            entityTypeBuilder.Property(x => x.Description);
           
        }
        
    }
}
