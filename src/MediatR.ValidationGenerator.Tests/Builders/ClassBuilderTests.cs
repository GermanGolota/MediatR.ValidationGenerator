using MediatR.ValidationGenerator.Builders;
using Xunit;

namespace MediatR.ValidationGenerator.Tests.Builders
{
    public class ClassBuilderTests
    {
        [Fact]
        public void Build_ShouldBuild()
        {
            //Arrange
            var builder = ClassBuilder.Create()
                .WithClassName("Test")
                .WithNamespace("TestNamespace")
                .UsingNamespace("System")
                .WithAccessModifier(AccessModifier.Public)
                .WithMethod((builder) =>
                {
                    return builder
                        .WithModifier(AccessModifier.Public)
                        .WithName("DoNothing")
                        .WithReturnType("void");
                });
            string expectedClass = @"
using System;

namespace TestNamespace
{
    public class Test
    {
        public void DoNothing()
        {
        }
    }
}
".RemoveFirstNewLine();
            //Act
            var actualClass = builder.Build();
            //Assert
            actualClass.Resolve(
                classStr => Assert.Equal(expectedClass, classStr),
                //should not get called
                _ => Assert.True(false)
                );
        }
    }
}
