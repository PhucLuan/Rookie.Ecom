using System;
using System.Collections.Generic;
using System.Text;

namespace Rookie.Ecom.Contracts
{
    public class PagedResponseModel<TModel>
    {
        public int CurrentPage { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<TModel> Items { get; set; }
    }
}
