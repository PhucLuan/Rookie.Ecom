using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rookie.Ecom.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.IdentityServer.Data
{
    public class Id4DbContext : IdentityDbContext<Id4Users, Id4Roles, string>
    {
        public Id4DbContext(DbContextOptions<Id4DbContext> options) : base(options)
        {

        }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>().HasOne(x => x.Users).WithMany(x => x.Addresses).HasForeignKey(x => x.UsersId);
        }
    }
}
