using MediatR.ValidationGenerator.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ValidationGenerator.RoslynUtils
{
    internal class BaseClassCollector
    {
        /// <summary>
        /// Collects all of the classes, that are being implemented by  <param name="desiredClass"></param>/>
        /// </summary>
        /// <param name="desiredClass"> Class base classes of which would be collected</param>
        /// <param name="classContext"> Classes, that are seen from <param name="desiredClass"> </param>
        /// <returns> 
        /// List of base class syntaxes.
        /// Note: This list is guaranteed to be sorted from closest parent to remotest
        /// </returns>
        public List<TypeDeclarationSyntax> Collect(TypeDeclarationSyntax desiredClass, List<TypeDeclarationSyntax> classContext)
        {
            _storedTypes = new List<TypeDeclarationSyntax>();
            CollectInternal(desiredClass, classContext);
            return _storedTypes;
        }

        private List<TypeDeclarationSyntax> _storedTypes { get; set; }
        private void CollectInternal(TypeDeclarationSyntax desiredClass, List<TypeDeclarationSyntax> classContext)
        {
            var baseClasses = desiredClass.BaseList.Types;
            if (baseClasses.IsNotNull())
            {
                var syntaxes = CollectBaseClassSyntaxes(desiredClass, classContext);
                _storedTypes = _storedTypes.Union(syntaxes).ToList();
                foreach (var newSyntax in syntaxes)
                {
                    CollectInternal(newSyntax, classContext);
                }
            }
        }

        private static List<TypeDeclarationSyntax> CollectBaseClassSyntaxes(
          TypeDeclarationSyntax desiredClass,
          List<TypeDeclarationSyntax> classContext
          )
        {
            List<TypeDeclarationSyntax> baseClasses = new List<TypeDeclarationSyntax>();

            var baseTypes = desiredClass.BaseList?.Types;
            if (baseTypes.HasValue)
            {
                var baseTypeNames = baseTypes.Value
                    .Select(x => x.Type)
                    .OfType<SimpleNameSyntax>();

                foreach (var baseTypeName in baseTypeNames)
                {
                    string baseName = baseTypeName.Identifier.Text;
                    TypeDeclarationSyntax baseClassSyntax = classContext
                        .FirstOrDefault(localClass => SyntaxUtils.IsTheSameClassNameOrGeneric(baseName, localClass.Identifier.Text));

                    if (baseClassSyntax.IsNotNull())
                    {
                        baseClasses.Add(baseClassSyntax);
                    }
                }
            }

            return baseClasses;
        }
    }
}
