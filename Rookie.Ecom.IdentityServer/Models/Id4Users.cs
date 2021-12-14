using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.IdentityServer.Models
{
    public class Id4Users : IdentityUser
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Address> Addresses { get; set; }

    }
}
