using Rookie.Ecom.DataAccessor.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.IntegrationTests.TestData
{
    public static class ArrangeData
    {
        public static Category Category() => new()
        {
            Name = "LA",
            Description = "Laptop",
        };
    }
}
