using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Rookie.Ecom.Test.Assertions;
using Rookie.Ecom.Test.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Test
{
    public static class AssertionExtensions
    {
        public static ActionResultAssertions Should(this ActionResult actualValue)
            => new ActionResultAssertions(actualValue);

        public static ValidationResultAssertions Should(this ValidationResult actualValue)
            => new ValidationResultAssertions(actualValue);
    }
}
