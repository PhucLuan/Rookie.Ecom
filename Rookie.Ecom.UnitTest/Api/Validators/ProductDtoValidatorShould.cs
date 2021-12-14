using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Dtos;
using Rookie.Ecom.Test.Validations;
using Rookie.Ecom.UnitTest.Api.Validators.TestData;
using Rookie.Ecom.WebApi.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rookie.Ecom.UnitTest.Api.Validators
{
    public class ProductDtoValidatorShould : BaseValidatorShould
    {
        private readonly ValidationTestRunner<ProductDtoValidator, ProductDto> _testRunner;
        private readonly Mock<IProductService> _productService;

        public ProductDtoValidatorShould()
        {
            _productService = new Mock<IProductService>();
            _testRunner = ValidationTestRunner
                .Create<ProductDtoValidator, ProductDto>(new ProductDtoValidator(_productService.Object));
        }

        [Theory]
        [MemberData(nameof(ProductTestData.ValidTexts), MemberType = typeof(ProductTestData))]
        public void NotHaveErrorWhenNameIsvalid(string name) =>
            _testRunner
                .For(m => m.Name = name)
                .ShouldNotHaveErrorsFor(m => m.Name);

        [Theory]
        [MemberData(nameof(ProductTestData.ValidTexts), MemberType = typeof(ProductTestData))]
        public void NotHaveErrorWhenPrefixIsvalid(string desc) =>
          _testRunner
              .For(m => m.Description = desc)
              .ShouldNotHaveErrorsFor(m => m.Description);

        [Theory]
        [MemberData(nameof(ProductTestData.InvalidNames), MemberType = typeof(ProductTestData))]
        public void HaveErrorWhenNameIsInvalid(string name, string errorMessage) =>
            _testRunner
                .For(m => m.Name = name)
                .ShouldHaveErrorsFor(m => m.Name, errorMessage);

        [Theory]
        [MemberData(nameof(ProductTestData.InvalidPrice), MemberType = typeof(ProductTestData))]
        public void HaveErrorWhenPriceIsInvalid(double price, string errorMessage) =>
            _testRunner
                .For(m => m.Price = price)
                .ShouldHaveErrorsFor(m => m.Price, errorMessage);

        [Theory]
        [MemberData(nameof(ProductTestData.ValidPrice), MemberType = typeof(ProductTestData))]
        public void NotHaveErrorWhenPriceIsvalid(double price)
        => _testRunner
                .For(m => m.Price = price)
                .ShouldNotHaveErrorsFor(m => m.Price);

        [Theory]
        [MemberData(nameof(ProductTestData.InvalidDiscount), MemberType = typeof(ProductTestData))]
        public void HaveErrorWhenDiscountIsInvalid(int discount, string errorMessage)
        => _testRunner
                .For(m => m.Discount = discount)
                .ShouldHaveErrorsFor(m => m.Discount,errorMessage);

        [Theory]
        [MemberData(nameof(ProductTestData.ValidDiscount), MemberType = typeof(ProductTestData))]
        public void NotHaveErrorWhenDiscountIsvalid(int discount)
        => _testRunner
                .For(m => m.Discount = discount)
                .ShouldNotHaveErrorsFor(m => m.Discount);

        [Theory]
        [MemberData(nameof(ProductTestData.InvalidCategory), MemberType = typeof(ProductTestData))]
        public void HaveErrorWhenCategoryIsInvalid(Guid CategoryID, string errorMessage)
        => _testRunner
                .For(m => m.CategoryId = CategoryID)
                .ShouldHaveErrorsFor(m => m.CategoryId, errorMessage);

    }
}
