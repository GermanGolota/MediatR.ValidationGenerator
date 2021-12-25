using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MediatR.ValidationGenerator.Extensions
{
    internal static class RoslynExtensions
    {
        public static string GetGlobalName(this ITypeSymbol type)
        {
            return type.MetadataName.GetFromGlobal(type.ContainingNamespace.ToDisplayString());
        }

        public static string GetFullName(this ITypeSymbol type)
        {
            return $"{type.ContainingNamespace.MetadataName}.{type.MetadataName}";
        }

        public static bool IsType(this ITypeSymbol type, string typeFullName)
        {
            return type.GetFullName().Equals(typeFullName);
        }

        public static bool IsNotAbstract(this TypeDeclarationSyntax syntax)
        {
            var members = syntax.Modifiers;
            return members.None(x => x.ToString() == "abstract");
        }

        public static bool IsConcreate(this TypeDeclarationSyntax syntax)
        {
            return syntax.IsNotAbstract() && !(syntax is InterfaceDeclarationSyntax);
        }

        public static List<IMethodSymbol> GetAllMethods(this ITypeSymbol type)
        {
            return type.GetAllMembers<IMethodSymbol>();
        }

        public static List<IPropertySymbol> GetAllProps(this ITypeSymbol type)
        {
            return type.GetAllMembers<IPropertySymbol>();
        }

        //TODO: Fix for overrides
        public static List<T> GetAllMembers<T>(this ITypeSymbol type) where T : ISymbol 
        {
            List<T> result = type.GetMembers().OfType<T>().ToList();

            var baseTypes = CollectBaseTypes(type);
            foreach (var baseType in baseTypes)
            {
                result.AddRange(GetAllMembers<T>(baseType));
            }

            return result;
        }

        /// <summary>
        /// Gets base types for CURRENT type - does not include base types of base types
        /// </summary>
        public static ImmutableArray<INamedTypeSymbol> CollectBaseTypes(this ITypeSymbol type)
        {
            var baseTypes = type.Interfaces;
            if (type.BaseType is not null)
            {
                baseTypes.Add(type.BaseType);
            }
            return baseTypes;
        }

        public static bool IsImplementing(this ITypeSymbol type, string interfaceName, string interfaceNamespace)
        {
            bool isImplementing = false;
            foreach (var interfaceType in type.AllInterfaces)
            {
                if (interfaceType.Name == interfaceName
                    && interfaceType.ContainingNamespace.Name == interfaceNamespace)
                {
                    isImplementing = true;
                    break;
                }
            }
            return isImplementing;
        }

        public static bool IsConcreate(this ITypeSymbol type)
        {
            return type.IsAbstract == false;
        }

    }
}
