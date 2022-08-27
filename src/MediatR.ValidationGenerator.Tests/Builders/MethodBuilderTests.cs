using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;

namespace MediatR.ValidationGenerator.Tests.Builders;

public class MethodBuilderTests
{
    [Fact]
    public void Build_ShouldCreateMethod_WhenTypeAndNameAreProvided()
    {
        //Arrange
        var builder = MethodBuilder.Create()
            .WithName("Sum")
            .WithReturnType("int")
            .WithModifier(AccessModifier.Public)
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
        ValueOrNull<string> actualResult = builder.Build();
        //Assert
        actualResult.Resolve(
        actual => Assert.Equal(expectedMethod, actual),
           //should not get called
           _ => Assert.True(false)
           );
    }
}
