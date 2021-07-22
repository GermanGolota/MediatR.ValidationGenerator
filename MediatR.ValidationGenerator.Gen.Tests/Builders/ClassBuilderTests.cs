﻿using MediatR.ValidationGenerator.Gen.Builders;
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
        public void Build_ShouldBuild()
        {
            //Arrange
            var builder = new ClassBuilder()
                .UsingNamespace("System")
                .WithNamespace("TestNamespace")
                .WithAccessModifier(AccessModifier.Public)
                .WithClassName("Test")
                .WithMethod(()=>
                {
                    return new MethodBuilder()
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
            Assert.Equal(expectedClass, actualClass);
        }
    }
}
