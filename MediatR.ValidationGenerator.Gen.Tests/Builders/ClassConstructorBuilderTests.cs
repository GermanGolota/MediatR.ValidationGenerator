using MediatR.ValidationGenerator.Gen.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MediatR.ValidationGenerator.Gen.Tests.Builders
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
                                        .WithBody((margin) =>
                                        {
                                            return new MethodBodyBuilder(margin)
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
            Assert.True(actual.HasValue);
            Assert.Equal(expected, actual.Value);
        }
    }
}
