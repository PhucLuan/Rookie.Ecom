using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Rookie.Ecom.DataAccessor.Models
{
    public class ApplicationRoles:IdentityRole
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IpAddress { get; set; }
    }
}
