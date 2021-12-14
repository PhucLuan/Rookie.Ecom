using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Rookie.Ecom.DataAccessor.Models.Admin;

namespace Rookie.Ecom.DataAccessor.Models
{
    public class ApplicationUsers:IdentityUser
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JoinIp { get; set; }
        public string Address { get; set; }
        public string Refference { get; set; }

        //public  ICollection<ProductComments> ProductCommentses { get; set; }
        public  ICollection<Orders> Orderses { get; set; }
    }
}
