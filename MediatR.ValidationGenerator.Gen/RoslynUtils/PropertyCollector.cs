using MediatR.ValidationGenerator.Gen.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RoslynUtils
{
    /// <summary>
    /// Collects members for class including members defined in parent classes
    /// </summary>
    internal static class PropertyCollector
    {
        public static IEnumerable<PropertyDeclarationSyntax> CollectFrom(
            ClassDeclarationSyntax desiredClass,
            List<TypeDeclarationSyntax> classContext
            )
        {
            var baseClassCollector = new BaseClassCollector();
            var currentPropList = desiredClass.Members.OfType<PropertyDeclarationSyntax>();
            var baseTypes = baseClassCollector.Collect(desiredClass, classContext);
            foreach (var baseType in baseTypes)
            {
                var currentClassProps = baseType.Members.OfType<PropertyDeclarationSyntax>();
                var newProps = currentClassProps.Where(prop => NotYetPresent(prop, currentPropList));
                currentPropList = currentPropList.Union(newProps).ToList();
            }
            return currentPropList;
        }

        private static bool NotYetPresent(PropertyDeclarationSyntax prop, IEnumerable<PropertyDeclarationSyntax> currentPropList)
        {
            return currentPropList.None(storedProp => storedProp.Identifier.Text == prop.Identifier.Text);
        }
    }
}
