using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Contracts.Dtos.Public
{
    public class HomePage
    {
        public List<CategoryDto> Categories { get; set; }
        public List<ProductListDto> ProductList { get; set; }
        public List<ProductListDto> FirstTab { get; set; }
        public List<ProductListDto> SecondTab { get; set; }
        public List<ProductListDto> ThirdTab { get; set; }
        public List<ProductListDto> FourthTab { get; set; }
        public List<BrandListDto> BrandList { get; set; }
        public List<string> CategoryNameList { get; set; }
    }
}
