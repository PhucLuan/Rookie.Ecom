using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Contracts.Dtos.Filter
{
    public class FilterBase
    {
        public string KeySearch { get; set; } = "";
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 2;
    }
}
