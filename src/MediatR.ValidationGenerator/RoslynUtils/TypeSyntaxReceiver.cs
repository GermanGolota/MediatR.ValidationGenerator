using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace MediatR.ValidationGenerator.RoslynUtils
{
    internal class TypeSyntaxReceiver : ISyntaxReceiver
    {
        public List<TypeDeclarationSyntax> Types { get; } = new();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is TypeDeclarationSyntax classNode)
            {
                Types.Add(classNode);
            }
        }
    }
}
