using MediatR.ValidationGenerator.Builders;
using Xunit;

namespace MediatR.ValidationGenerator.Tests.Builders
{
    public class ClassConstructorBuilderTests
    {
        [Fact]
        public void Build_ShouldBuild_WhenClassNameIsProvided()
        {
            //Arrange
            var constructorBuilder = new ClassConstructorBuilder()
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
            var actualResult = constructorBuilder.Build();
            //Assert
            actualResult.Resolve(
               actual => Assert.Equal(expected, actual),
               //should not get called
               _ => Assert.True(false)
               );
        }
    }
}
