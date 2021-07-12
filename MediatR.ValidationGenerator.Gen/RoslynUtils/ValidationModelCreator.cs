using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RoslynUtils
{
    internal static class ValidationModelCreator
    {
        public static List<RequestValidationModel> GetValidationModels(
            List<TypeDeclarationSyntax> classContext,
            IEnumerable<ClassDeclarationSyntax> requestClasses
            )
        {
            List<RequestValidationModel> validationModels = new List<RequestValidationModel>();

            foreach (var requestClass in requestClasses)
            {
                var requestModel = new RequestValidationModel(requestClass);
                var propertyDict = requestModel.PropertyToSupportedAttributes;

                var props = PropertyCollector.CollectFrom(requestClass, classContext);
                foreach (var prop in props)
                {
                    foreach (var attribute in prop.AttributeLists)
                    {
                        var attrs = attribute.Attributes;
                        foreach (var actualAttribute in attrs)
                        {
                            if (AttributeService.AttributeIsSupported(actualAttribute))
                            {
                                if (propertyDict.ContainsKey(prop))
                                {
                                    propertyDict[prop].Add(actualAttribute);
                                }
                                else
                                {
                                    propertyDict.Add(prop, new List<AttributeSyntax> { actualAttribute });
                                }
                            }
                        }
                    }
                }

                if (propertyDict.Keys.Any())
                {
                    validationModels.Add(requestModel);
                }
            }

            return validationModels;
        }
    }
}
