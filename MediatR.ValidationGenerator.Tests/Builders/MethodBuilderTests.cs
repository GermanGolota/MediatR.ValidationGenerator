using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Xunit;

namespace MediatR.ValidationGenerator.Tests.Builders
{
    public class MethodBuilderTests
    {
        [Fact]
        public void Build_ShouldCreateMethod_WhenTypeAndNameAreProvided()
        {
            //Arrange
            var builder = new MethodBuilder()
                .WithModifier(AccessModifier.Public)
                .WithReturnType("int")
                .WithName("Sum")
                .WithParameter("int", "a")
                .WithParameter("int", "b")
                .WithBody((body) =>
                {
                    return body
                        .AppendLine("return a + b");
                });

            string expectedMethod = @"
public int Sum(int a, int b)
{
    return a + b;
}
"
.RemoveFirstNewLine();
            //Act
            ValueOrNull<string> actualMethod = builder.Build();
            //Assert
            Assert.True(actualMethod.HasValue);
            Assert.Equal(expectedMethod, actualMethod.Value);
        }

        [Fact]
        public void Build_ShouldNotCreateMethod_WhenNoNameIsProvided()
        {
            //Arrange
            var builder = new MethodBuilder()
                .WithModifier(AccessModifier.Public)
                .WithReturnType("void");
            //Act
            ValueOrNull<string> result = builder.Build();
            //Assert
            Assert.True(result.IsNull);
        }

        [Fact]
        public void Build_ShouldNotCreateMethod_WhenNoReturnTypeIsProvided()
        {
            //Arrange
            var builder = new MethodBuilder()
                .WithModifier(AccessModifier.Public)
                .WithName("DoNothing");
            //Act
            ValueOrNull<string> result = builder.Build();
            //Assert
            Assert.True(result.IsNull);
        }
    }
}
