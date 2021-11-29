using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MediatR.ValidationGenerator.Gen.Tests
{
    public class SourceGeneratorTests
    {
        [Fact]
        public void Execute_ShouldNotThrow_WhenRequestsExist()
        {
            //Arrange
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
}
"
            };
            Compilation inputCompilation = CreateCompilation(sources);
            SourceGenerator generator = new SourceGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            //Act
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
            GeneratorDriverRunResult result = driver.GetRunResult();
            //Assert
            //do not throws
        }

        private static Compilation CreateCompilation(List<string> sources)
        {
            return CSharpCompilation.Create("compilation",
                sources.Select(source => CSharpSyntaxTree.ParseText(source)),
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}
