using AutoMapper;
using FluentAssertions;
using Rookie.Ecom.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rookie.Ecom.UnitTest
{
    public class AutoMapperProfileShould
    {
        [Fact]
        public void BeValid()
        {
            // Arrange
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            IMapper mapper = config.CreateMapper();

            // Act
            Action act = () => mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Assert
            act.Should().NotThrow();
        }
    }
}
