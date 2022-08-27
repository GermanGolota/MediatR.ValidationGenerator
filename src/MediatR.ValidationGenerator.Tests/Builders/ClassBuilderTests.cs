using MediatR.ValidationGenerator.Builders;

namespace MediatR.ValidationGenerator.Tests.Builders;

public class ClassBuilderTests
{
    [Fact]
    public void Build_ShouldBuild_WithOneMethod()
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


    [Fact]
    public void Build_ShouldBuild_WithFieldsAndCtor()
    {
        //Arrange
        var builder = ClassBuilder.Create()
            .WithClassName("DomainModelNameService")
            .WithNamespace("Application")
            .Implementing("IDomainModelNameService")
            .UsingNamespace("System")
            .UsingNamespace("Microsoft.Extensions.Caching.Abstractions")
            .WithAccessModifier(AccessModifier.Public)
            .WithField(new FieldModel("_cache", "IMemoryCache"))
            .WithConstructor((ctor) =>
                ctor
                    .WithModifier(AccessModifier.Public)
                    .WithParameter("IMemoryCache", "cache")
                    .WithBody(body => body.AppendLine("_cache = cache"))
            );
        string expectedClass = @"
using System;
using Microsoft.Extensions.Caching.Abstractions;

namespace Application
{
    public class DomainModelNameService : IDomainModelNameService
    {
        private IMemoryCache _cache;

        public DomainModelNameService(IMemoryCache cache)
        {
            _cache = cache;
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
