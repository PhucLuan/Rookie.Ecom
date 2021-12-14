using System;
using System.Collections.Generic;
using System.Text;

namespace Rookie.Ecom.Contracts
{
    public class BaseQueryCriteria
    {
        public int Limit { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
