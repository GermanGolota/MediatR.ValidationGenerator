using MediatR.ValidationGenerator.Gen.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RoslynUtils
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
                foreach (var baseClass in baseTypes)
                {
                    var type = baseClass.Type;
                    var name = type as SimpleNameSyntax;
                    if (name.IsNotNull())
                    {
                        var identifier = name.Identifier;
                        string baseTypeName = identifier.Text;

                        TypeDeclarationSyntax baseClassSyntax = null;

                        foreach (var localClass in classContext)
                        {
                            var localClassName = localClass.Identifier.Text;
                            if (SyntaxUtils.IsTheSameClassNameOrGeneric(baseTypeName, localClassName))
                            {
                                baseClassSyntax = localClass;
                                break;
                            }
                        }

                        if (baseClassSyntax.IsNotNull())
                        {
                            baseClasses.Add(baseClassSyntax);
                        }
                    }
                }
            }

            return baseClasses;
        }
    }
}
