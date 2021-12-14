using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Contracts.Dtos.Public
{
    public class CategoryMenuDto
    {
        public string Name { get; set; }
        public List<Category> categoryChildMenuDtos { get; set; }
    }
}
