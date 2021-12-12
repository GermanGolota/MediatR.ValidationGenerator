using MediatR.ValidationGenerator.RuleGenerators;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MediatR.ValidationGenerator.Tests.RuleGenerators
{
    public class AttributeHelperTests
    {
        [Fact]
        public void GetName_ShouldRemoveAttributeFromAttributeName()
        {
            //Arrange
            string expected = "Required";
            //Act
            bool result = AttributeHelper.IsTheSameAttribute(nameof(RequiredAttribute), expected);
            //Assert
            Assert.True(result);
        }
    }
}
