using Rookie.Ecom.Contracts.Dtos.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Contracts.Dtos
{
    public class FilterProductsModel : FilterBase
    {
        public string CategoryId { get; set; } = "";
        public string BrandId { get; set; } = "";
        public string OrderProperty { get; set; } = "";
        public bool Desc { get; set; } = true;
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }
    public class ListFilterProduct
    {
        public ListFilterProduct()
        {
            //Images = new IFormFile();
            CategoryList = new List<MySelectListItem>();
            BrandList = new List<MySelectListItem>();
        }
        public List<MySelectListItem> CategoryList { get; set; }
        public List<MySelectListItem> BrandList { get; set; }
    }
}
