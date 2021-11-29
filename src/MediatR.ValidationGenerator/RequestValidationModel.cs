using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace MediatR.ValidationGenerator
{
    public class RequestValidationModel
    {
        public RequestValidationModel(ClassDeclarationSyntax requestClass)
        {
            RequestClass = requestClass;
        }

        public ClassDeclarationSyntax RequestClass { get; set; }
        public Dictionary<PropertyDeclarationSyntax, List<AttributeSyntax>> PropertyToSupportedAttributes { get; set; } =
            new Dictionary<PropertyDeclarationSyntax, List<AttributeSyntax>>();

        public string ValidatorName => $"{RequestClass.Identifier.Text}Validator";
    }
}
