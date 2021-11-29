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
            string actual = AttributeHelper.GetProperName(nameof(RequiredAttribute));
            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
