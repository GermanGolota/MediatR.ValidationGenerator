using MediatR.ValidationGenerator.Gen.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RoslynUtils
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
