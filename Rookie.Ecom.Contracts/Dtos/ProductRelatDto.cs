using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class ProductRelatDto
    {
        public Product product { get; set; }
        public int count { get; set; }
        public List<string> ImagePaths { get; set; }
    }
}
