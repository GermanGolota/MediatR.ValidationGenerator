using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using MediatR.ValidationGenerator.Gen.Extensions;
using System.Diagnostics;
using System.Linq;
using MediatR.ValidationGenerator.Gen.RoslynUtils;

namespace MediatR.ValidationGenerator.Gen
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new TypeSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = context.SyntaxReceiver as TypeSyntaxReceiver;
            var classContext = receiver.Types;
            var requestClasses = ClassSorter.SortClassesThatImplement(classContext, "IRequest");
            var validationModels = ValidationModelCreator.GetValidationModels(classContext, requestClasses);
            foreach (var validationModel in validationModels)
            {
                var creationResult = RequestValidatorCreator.CreateValidatorFor(validationModel);
                if (creationResult.HasValue)
                {
                    context.AddSource(validationModel.ValidatorName, creationResult.Value);
                }
            }
        }
    }
}
