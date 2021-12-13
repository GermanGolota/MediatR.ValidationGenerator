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
                        .WithName("DoNothing")
                        .WithReturnType("void")
                        .WithModifier(AccessModifier.Public);
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
            Assert.Equal(expectedClass, actualClass);
        }
    }
}
