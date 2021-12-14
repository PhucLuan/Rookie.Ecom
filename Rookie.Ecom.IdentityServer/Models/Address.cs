using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.IdentityServer.Models
{
    public class Address : BaseEntity
    {
        public string CustomerAddress { get; set; }
        public Id4Users Users { get; set; }
        public string UsersId { get; set; }
    }
}
