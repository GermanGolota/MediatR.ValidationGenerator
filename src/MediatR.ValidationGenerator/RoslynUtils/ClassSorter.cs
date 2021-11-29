using MediatR.ValidationGenerator.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ValidationGenerator.RoslynUtils
{
    internal static class ClassSorter
    {
        public static IEnumerable<ClassDeclarationSyntax> SortClassesThatImplement(List<TypeDeclarationSyntax> classes, string className)
        {
            Dictionary<bool, List<TypeDeclarationSyntax>> classInheritsFromIRequest = new Dictionary<bool, List<TypeDeclarationSyntax>>
            {
                {false, new List<TypeDeclarationSyntax>() },
                {true, new List<TypeDeclarationSyntax>() }
            };

            foreach (var receiverClass in classes)
            {
                bool inherits = SyntaxUtils.InheritsFrom(receiverClass, className);
                classInheritsFromIRequest[inherits].Add(receiverClass);
            }

            while (true)
            {
                bool newAppeared = false;

                List<TypeDeclarationSyntax> notYetInherited = classInheritsFromIRequest[false];
                for (int i = 0; i < notYetInherited.Count; i++)
                {
                    var classItem = notYetInherited[i];
                    if (InheritsFrom(classItem, classInheritsFromIRequest, className))
                    {
                        classInheritsFromIRequest[false].Remove(classItem);
                        classInheritsFromIRequest[true].Add(classItem);

                        newAppeared = true;
                        i--;
                    }
                }

                if (newAppeared == false)
                {
                    break;
                }
            }

            var classesThatImplementIRequested = classInheritsFromIRequest[true];
            return classesThatImplementIRequested.Where(x => x.IsConcreate()).Cast<ClassDeclarationSyntax>();
        }

        private static bool InheritsFrom(TypeDeclarationSyntax declaration, Dictionary<bool, List<TypeDeclarationSyntax>> known, string className)
        {
            return SyntaxUtils.InheritsFrom(declaration, className) || InheritsFromKnown(declaration, known);
        }

        private static bool InheritsFromKnown(TypeDeclarationSyntax declaration, Dictionary<bool, List<TypeDeclarationSyntax>> known)
        {
            var alreadyInherits = known[true];
            bool result = false;
            var types = declaration?.BaseList?.Types;
            if (types.HasValue)
            {
                foreach (var baseType in types.Value)
                {
                    string baseTypeName = baseType.ToString();
                    foreach (var inheritedClass in alreadyInherits)
                    {
                        string inheritedClassName = inheritedClass.Identifier.ToString();
                        if (SyntaxUtils.IsTheSameClassNameOrGeneric(inheritedClassName, baseTypeName))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
