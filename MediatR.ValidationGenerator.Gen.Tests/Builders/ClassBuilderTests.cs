using MediatR.ValidationGenerator.Gen.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MediatR.ValidationGenerator.Gen.Tests.Builders
{
    public class ClassBuilderTests
    {
        [Fact]
        public void Build_ShouldBuild_WhenAllRequirementsAreProvided()
        {
            //Arrange
            var builder = new ClassBuilder()
                .UsingNamespace("System")
                .WithNamespace("TestNamespace")
                .WithAccessModifier(AccessModifier.Public)
                .WithClassName("Test")
                .WithMethod((method) =>
                {
                    return method
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
            Assert.True(actualClass.HasValue);
            Assert.Equal(expectedClass, actualClass.Value);
        }

        [Fact]
        public void Build_ShouldBuildWithConstructor()
        {
            //Arrange
            var classBuilder = new ClassBuilder()
                                .WithAccessModifier(AccessModifier.Public)
                                .WithClassName("Test")
                                .WithNamespace("TestName")
                                .WithConstructor((ctor) =>
                                {
                                    return ctor
                                        .WithModifier(AccessModifier.Public);
                                });
            string expected = @"
namespace TestName
{
    public class Test
    {
        public Test()
        {
        }
    }
}
".RemoveFirstNewLine();
            //Act
            var actual = classBuilder.Build();
            //Assert
            Assert.True(actual.HasValue);
            Assert.Equal(expected, actual.Value);
        }
    }
}
