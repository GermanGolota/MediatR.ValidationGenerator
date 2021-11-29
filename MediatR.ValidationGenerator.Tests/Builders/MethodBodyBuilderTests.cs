using MediatR.ValidationGenerator.Builders;
using Xunit;

namespace MediatR.ValidationGenerator.Tests.Builders
{
    public class MethodBodyBuilderTests
    {
        [Fact]
        public void Build_ShouldAddAllLines()
        {
            //Arrange
            var builder = new MethodBodyBuilder();
            string expectedBody = @"
{
    var a = 3;
    var b = 4;
        var c = a + b;
}
".RemoveFirstNewLine();
            //Act
            builder.AppendLine("var a = 3;")
                .AppendLine("var b = 4;")
                .AppendLine("var c = a + b;", 1);

            var actualBody = builder.Build();
            //Assert
            Assert.True(actualBody.HasValue);
            Assert.Equal(expectedBody, actualBody);
        }
    }
}
