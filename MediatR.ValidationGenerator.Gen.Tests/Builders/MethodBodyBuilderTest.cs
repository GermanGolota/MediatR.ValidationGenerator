using MediatR.ValidationGenerator.Gen.Builders;
using System;
using Xunit;

namespace MediatR.ValidationGenerator.Gen.Tests
{
    public class MethodBodyBuilderTest
    {
        [Fact]
        public void Build_ShouldAddAllLines()
        {
            //Arrange
            var builder = new MethodBodyBuilder();
            string expectedBody = "{\r\n\tvar a = 3;\r\n\tvar b = 4;\r\n\t\tvar c = a + b;\r\n}\r\n";
            //Act
            builder.AppendLine("var a = 3;")
                .AppendLine("var b = 4;")
                .AppendLine("var c = a + b;", 1);

            string actualBody = builder.Build();
            //Assert
            Assert.Equal(expectedBody, actualBody);
        }
    }
}
