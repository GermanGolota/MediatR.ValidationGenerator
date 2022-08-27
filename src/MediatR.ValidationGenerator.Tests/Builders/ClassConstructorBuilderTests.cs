using MediatR.ValidationGenerator.Builders;

namespace MediatR.ValidationGenerator.Tests.Builders;

public class ClassConstructorBuilderTests
{
    [Fact]
    public void Build_ShouldBuild_WhenClassNameIsProvided()
    {
        //Arrange
        var constructorBuilder = ClassConstructorBuilder.Create()
                                    .WithClassName("Test")
                                    .WithParameter("int", "margin")
                                    .WithModifier(AccessModifier.Public)
                                    .WithBody((body) =>
                                    {
                                        return body
                                            .AppendLine("var a = 2 + margin");
                                    });
        string expected = @"
public Test(int margin)
{
    var a = 2 + margin;
}
".RemoveFirstNewLine();
        //Act
        var actual = constructorBuilder.Build();
        //Assert
        Assert.Equal(expected, actual);
    }
}
