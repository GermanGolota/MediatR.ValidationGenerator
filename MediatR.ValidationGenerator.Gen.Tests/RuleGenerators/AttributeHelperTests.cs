using MediatR.ValidationGenerator.Gen.RuleGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MediatR.ValidationGenerator.Gen.Tests.RuleGenerators
{
    public class AttributeHelperTests
    {
        [Fact]
        public void GetName_ShouldRemoveAttributeFromAttributeName()
        {
            //Arrange
            string expected = "Required";
            //Act
            string actual = AttributeHelper.GetProperName(nameof(RequiredAttribute));
            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
