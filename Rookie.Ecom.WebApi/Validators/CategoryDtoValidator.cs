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
    public class CategoryDtoValidator : BaseValidator<CategoryDto>
    {
        public CategoryDtoValidator(ICategoryService categoryService)
        {
            RuleFor(m => m.Id)
                 .NotNull()
                 .WithMessage(x => string.Format(ErrorTypes.Common.RequiredError, nameof(x.Id)));

            RuleFor(m => m.Name)
                  .NotEmpty()
                  .WithMessage(x => string.Format(ErrorTypes.Common.RequiredError, nameof(x.Name)));

            RuleFor(m => m.Name)
               .MaximumLength(ValidationRules.CategoryRules.MaxLenghCharactersForName)
               .WithMessage(string.Format(ErrorTypes.Common.MaxLengthError, ValidationRules.CategoryRules.MaxLenghCharactersForName))
               .When(m => !string.IsNullOrWhiteSpace(m.Name));

            RuleFor(m => m.Description)
               .MaximumLength(ValidationRules.CategoryRules.MaxLenghCharactersForDesc)
               .WithMessage(string.Format(ErrorTypes.Common.MaxLengthError, ValidationRules.CategoryRules.MaxLenghCharactersForDesc))
               .When(m => !string.IsNullOrWhiteSpace(m.Description));

          //  RuleFor(x => x).MustAsync(
          //   async (dto, cancellation) =>
          //   {
          //       var exit = await categoryService.GetByNameAsync(dto.Name);
          //       return exit != null && exit.Id != dto.Id;
          //   }
          //).WithMessage("Duplicate record");
        }
    }
}
