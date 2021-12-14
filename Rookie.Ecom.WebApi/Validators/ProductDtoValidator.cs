using FluentValidation;
using Rookie.Ecom.Business.Interfaces;
using Rookie.Ecom.Contracts.Constants;
using Rookie.Ecom.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rookie.Ecom.WebApi.Validators
{
    public class ProductDtoValidator : BaseValidator<ProductDto>
    {
        public ProductDtoValidator(IProductService productService)
        {
            RuleFor(m => m.Id)
                 .NotNull()
                 .WithMessage(x => string.Format(ErrorTypes.Common.RequiredError, nameof(x.Id)));

            RuleFor(m => m.CategoryId)
                 .NotEqual(default(Guid))
                 .WithMessage(x => string.Format(ErrorTypes.Common.RequiredError, nameof(x.CategoryId)));

            RuleFor(m => m.BrandId)
                 .NotEqual(default(Guid))
                 .WithMessage(x => string.Format(ErrorTypes.Common.RequiredError, nameof(x.BrandId)));

            RuleFor(m => m.UnitId)
                 .NotEqual(default(Guid))
                 .WithMessage(x => string.Format(ErrorTypes.Common.RequiredError, nameof(x.UnitId)));

            RuleFor(m => m.Name)
               .MaximumLength(ValidationRules.CategoryRules.MaxLenghCharactersForName)
               .WithMessage(string.Format(ErrorTypes.Common.MaxLengthError, ValidationRules.CategoryRules.MaxLenghCharactersForName))
               .When(m => !string.IsNullOrWhiteSpace(m.Name));

            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage(x => string.Format(ErrorTypes.Common.RequiredError, nameof(x.Name)));

            RuleFor(m => m.Code)
                 .NotEmpty()
                 .WithMessage(x => string.Format(ErrorTypes.Common.RequiredError, nameof(x.Code)));

            RuleFor(m => m.Tag)
                 .NotEmpty()
                 .WithMessage(x => string.Format(ErrorTypes.Common.RequiredError, nameof(x.Tag)));

            RuleFor(m => m.Description)
               .MaximumLength(ValidationRules.CategoryRules.MaxLenghCharactersForDesc)
               .WithMessage(string.Format(ErrorTypes.Common.MaxLengthError, ValidationRules.CategoryRules.MaxLenghCharactersForDesc))
               .When(m => !string.IsNullOrWhiteSpace(m.Description));

            RuleFor(m => m.Price)
                .GreaterThan(0)
                .WithMessage(string.Format(ErrorTypes.Common.NumberGreaterThanError, "Price", ValidationRules.ProductRules.Minvalue));

            RuleFor(m => m.Discount)
                .GreaterThanOrEqualTo(0)
                .WithMessage(string.Format(ErrorTypes.Common.GreaterThanOrEqual, "Discount", ValidationRules.ProductRules.Minvalue));

            RuleFor(m => m.Discount)
                .LessThanOrEqualTo(100)
                .WithMessage(string.Format(ErrorTypes.Common.FromNumberLessThanToNumberError, "Discount", ValidationRules.ProductRules.Maxdiscountvalue));
            
            RuleFor(m => m.ProductStock)
                .GreaterThanOrEqualTo(0)
                .WithMessage(string.Format(ErrorTypes.Common.GreaterThanOrEqual, "ProductStock", ValidationRules.ProductRules.Minvalue));

            //RuleFor(x => x).MustAsync(
            //async (dto, cancellation) =>
            //{
            //    var exit = await productService.GetByNameAsync(dto.Name);
            //    return exit != null && exit.Id != dto.Id;
            //}
            //).WithMessage("Duplicate record");
        }
    }
}
