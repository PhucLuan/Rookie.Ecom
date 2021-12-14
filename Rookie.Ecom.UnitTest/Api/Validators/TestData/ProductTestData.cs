using Rookie.Ecom.Contracts.Constants;
using Rookie.Ecom.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.UnitTest.Api.Validators.TestData
{
    public class ProductTestData
    {
        public static IEnumerable<object[]> ValidTexts()
        {
            return new object[][]
            {
                new object[] { "product name" },
                new object[] { "product" },
            };
        }

        public static IEnumerable<object[]> InvalidNames()
        {
            return new object[][]
            {
                new object[]
                {
                    "Mattis adipiscing magnis montes semper. Amet risus venenatis. " +
                    "Suspendisse pede pharetra nec praesent cursus nibh tortor pharetra ante commodo et."+
                    "Suspendisse pede pharetra nec praesent cursus nibh tortor pharetra ante commodo et.",
                    string.Format(ErrorTypes.Common.MaxLengthError, ValidationRules.CategoryRules.MaxLenghCharactersForName)
                },
                new object[] { "", string.Format(ErrorTypes.Common.RequiredError, nameof(ProductDto.Name))},
                new object[] { null, string.Format(ErrorTypes.Common.RequiredError, nameof(ProductDto.Name))},
            };
        }

        public static IEnumerable<object[]> InvalidDescs()
        {
            return new object[][]
            {
                new object[]
                {
                    "Mattis adipiscing magnis montes semper. Amet risus venenatis." +
                    "Mattis adipiscing magnis montes semper. Amet risus venenatis." +
                    "Mattis adipiscing magnis montes semper. Amet risus venenatis." +
                    "Mattis adipiscing magnis montes semper. Amet risus venenatis." +
                    "Mattis adipiscing magnis montes semper. Amet risus venenatis." +
                    "Mattis adipiscing magnis montes semper. Amet risus venenatis. " ,
                    string.Format(ErrorTypes.Common.MaxLengthError, ValidationRules.ProductRules.MaxLenghCharactersForDesc)
                }
            };
        }

        public static IEnumerable<object[]> InvalidPrice()
        {
            return new object[][]
            {
                new object[]
                {0,string.Format(ErrorTypes.Common.NumberGreaterThanError,"Price", ValidationRules.ProductRules.Minvalue)},
                new object[] { -10, string.Format(ErrorTypes.Common.NumberGreaterThanError,"Price", ValidationRules.ProductRules.Minvalue)},
            };
        }

        public static IEnumerable<object[]> ValidPrice()
        {
            return new object[][]
            {
                new object[]{10000,},
            };
        }

        public static IEnumerable<object[]> InvalidDiscount()
        {
            return new object[][]
            {
                new object[]
                {-1,string.Format(ErrorTypes.Common.GreaterThanOrEqual,"Discount", ValidationRules.ProductRules.Minvalue)},
                new object[] { 101, string.Format(ErrorTypes.Common.FromNumberLessThanToNumberError, "Discount", ValidationRules.ProductRules.Maxdiscountvalue) },
            };
        }

        public static IEnumerable<object[]> ValidDiscount()
        {
            return new object[][]
            {
                new object[]{0,},
                new object[]{100,},
            };
        }

        public static IEnumerable<object[]> InvalidCategory()
        {
            return new object[][]
            {
                new object[]
                {default(Guid),string.Format(ErrorTypes.Common.RequiredError,"CategoryId")}
            };
        }
    }
}
