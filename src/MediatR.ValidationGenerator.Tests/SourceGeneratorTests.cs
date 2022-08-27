using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MediatR.ValidationGenerator.Tests;

public class SourceGeneratorTests
{
    [Fact]
    public void Execute_ShouldNotThrow_WhenRequestsExist()
    {
        //Arrange
        #region SourceCodes
        List<string> sources = new List<string>
        {
            @"
using System;
using System.ComponentModel.DataAnnotations;

namespace MediatR.ValidationGenerator.Gen.Tests.TestCommands
{
    public abstract class BaseRequest<T> : IRequest<T>
    {
        [Required]
        public Guid RequestId { get; set; }
    }
}
",
            @"
using System;
using System.ComponentModel.DataAnnotations;

namespace MediatR.ValidationGenerator.Gen.Tests.TestCommands
{
    public class ValueCommand : BaseRequest<string>
    {
        [Required]
        [RegularExpression(""[A-Z,a-z,0-9,-]"")]
        public Guid ValueId { get; set; }
    }
}
",
            @"
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ValidationGenerator.Gen.Tests.TestCommands
{
    public class ValueCommandHandler : IRequestHandler<ValueCommand, string>
    {
        public Task<string> Handle(ValueCommand request, CancellationToken cancellationToken)
        {
            Guid id = request.ValueId;

            var length = int.Parse(id.ToString()[0].ToString());

            string value = request.ValueId.ToString().Substring(length);
            return Task.FromResult(value);
        }
    }
},
",
            @"
namespace MediatR.ValidationGenerator.Gen.Tests
{
    public static class Program
    {
        public static void Main(string[] args)
        {}
    }
}
"
        };
        #endregion
        Compilation inputCompilation = CreateCompilation(sources);
        var generator = new ValidatorsGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        //Act
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
        GeneratorDriverRunResult result = driver.GetRunResult();
        //Assert
        Assert.Empty(diagnostics);
        Assert.Empty(result.Diagnostics);
        Assert.NotEmpty(result.GeneratedTrees);
    }

    private static Compilation CreateCompilation(List<string> sources)
    {
        var types = new[] {
            typeof(Binder),
            typeof(IBaseRequest),
            typeof(RegularExpressionAttribute)
        };

        var locations = types.Select(x => x.GetTypeInfo().Assembly.Location)
            .Union(GetStandardAssemblies())
            .Distinct();

        var references = locations.Select(location => MetadataReference.CreateFromFile(location));
        var sourceTrees = sources.Select(source => CSharpSyntaxTree.ParseText(source));
        return CSharpCompilation.Create("compilation",
            sourceTrees,
            references,
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }

    private static IEnumerable<string> GetStandardAssemblies()
    {
        //TODO: Replace with Assembly.Load
        var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
        var standardAssemblyNames = new[]
        {
             "mscorlib.dll",
             "System.dll",
             "System.Core.dll",
             "System.Runtime.dll",
             "netstandard.dll"
        };
        return standardAssemblyNames.Select(x => Path.Combine(assemblyPath, x));
    }
}
