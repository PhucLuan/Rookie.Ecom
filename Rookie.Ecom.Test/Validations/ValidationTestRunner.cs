using Rookie.Ecom.WebApi.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Test.Validations
{
    public static class ValidationTestRunner
    {
        public static ValidationTestRunner<TValidator, TModel> Create<TValidator, TModel>()
            where TValidator : BaseValidator<TModel>, new()
            where TModel : class, new() =>
            new ValidationTestRunner<TValidator, TModel>(new TValidator());

        public static ValidationTestRunner<TValidator, TModel> Create<TValidator, TModel>(TValidator validator)
            where TValidator : BaseValidator<TModel>
            where TModel : class, new() =>
            new ValidationTestRunner<TValidator, TModel>(validator);
    }
}
