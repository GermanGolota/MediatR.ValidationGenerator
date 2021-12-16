using MediatR.ValidationGenerator.Builders.Abstractions;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MediatR.ValidationGenerator.Builders
{
    public interface IClassNameSelector
    {
        IClassNameSpaceSelector WithClassName(string className);
    }

    public interface IClassNameSpaceSelector
    {
        IClassBuilder WithNamespace(string classNamespace);
    }

    public interface IClassBuilder : IBuilder
    {
        IClassBuilder AsPartial();
        IClassBuilder WithAccessModifier(AccessModifier modifier);
        IClassBuilder WithMethod(Func<IMethodNameSelector, IMethodBuilder> methodBuilder);
        IClassBuilder WithConstructor(Func<IClassConstructorBuilder, IClassConstructorBuilder> constructorBuilder);
        IClassBuilder UsingNamespace(string usedNamespace);
        IClassBuilder Implementing(string className);
    }

    public class ClassBuilder : IClassNameSpaceSelector, IClassNameSelector, IClassBuilder
    {

        public static IClassNameSelector Create()
        {
            return new ClassBuilder();
        }

        private ClassBuilder()
        {

        }

        private List<string> _implementsList = new List<string>();
        private List<string> _usedNamespaces = new List<string>();

        private bool _isPartial = false;

        private List<IMethodBuilder> _methods = new List<IMethodBuilder>();
        private IClassConstructorBuilder _constructor;

        private string _className;
        private AccessModifier _modifier = AccessModifier.Public;
        private string _classNamespace;

        public IClassNameSpaceSelector WithClassName(string className)
        {
            _className = className;
            return this;
        }
        public IClassBuilder WithNamespace(string classNamespace)
        {
            _classNamespace = classNamespace;
            return this;
        }
        public IClassBuilder WithAccessModifier(AccessModifier modifier)
        {
            _modifier = modifier;
            return this;
        }
        public IClassBuilder WithMethod(Func<IMethodNameSelector, IMethodBuilder> methodBuilder)
        {
            var initial = MethodBuilder.Create(2);
            var method = methodBuilder(initial);
            _methods.Add(method);
            return this;
        }
        public IClassBuilder WithConstructor(Func<IClassConstructorBuilder, IClassConstructorBuilder> constructorBuilder)
        {
            IClassConstructorBuilder initalCtor = ClassConstructorBuilder.Create(2)
                .WithClassName(_className);
            _constructor = constructorBuilder(initalCtor);
            return this;
        }

        public IClassBuilder UsingNamespace(string usedNamespace)
        {
            _usedNamespaces.Add(usedNamespace);
            return this;
        }

        public IClassBuilder Implementing(string className)
        {
            _implementsList.Add(className);
            return this;
        }

        public IClassBuilder AsPartial()
        {
            _isPartial = true;
            return this;
        }

        private string BuildClassBody()
        {
            StringBuilder classBodyBuilder = new StringBuilder();
            string signature = BuildSignature(_modifier, _className, _implementsList, _isPartial);
            classBodyBuilder.AppendLine($"{BuilderUtils.TAB}{signature}");
            classBodyBuilder.AppendLine(BuilderUtils.TAB + "{");
            var ctor = BuildConstructor();
            classBodyBuilder.Append(ctor);
            string methods = BuildMethods(_methods);
            classBodyBuilder.Append(methods);
            classBodyBuilder.AppendLine(BuilderUtils.TAB + "}");
            return classBodyBuilder.ToString();
        }

        private string BuildConstructor()
        {
            string result = "";
            if (_constructor.IsNotNull())
            {
                result = _constructor.Build();
            }
            return result;
        }

        [Pure]
        private string BuildMethods(List<IMethodBuilder> methods)
        {
            StringBuilder classBodyBuilder = new StringBuilder();
            foreach (var method in methods)
            {
                classBodyBuilder.Append(method.Build());
            }
            return classBodyBuilder.ToString();
        }

        [Pure]
        private string BuildSignature(AccessModifier modifier, string nameOfClass, List<string> implementsList, bool isPartial)
        {
            StringBuilder signatureBuilder = new StringBuilder();
            string modifierStr = modifier.ToString().ToLower();
            signatureBuilder.Append(modifierStr);
            if (isPartial)
            {
                signatureBuilder.Append(" partial");
            }
            signatureBuilder.Append($" class {nameOfClass}");
            if (_implementsList.Count > 0)
            {
                string implements = $" : {string.Join(",", implementsList)}";
                signatureBuilder.Append(implements);
            }
            return signatureBuilder.ToString();
        }

        [Pure]
        private string BuildUsings(List<string> namespaceList)
        {
            StringBuilder namespaceBuilder = new StringBuilder();
            for (int i = 0; i < namespaceList.Count; i++)
            {
                var usedNamespace = namespaceList[i];
                if (usedNamespace.NotEndsWith(";"))
                {
                    usedNamespace = $"using {usedNamespace};";
                }
                namespaceBuilder.AppendLine(usedNamespace);
            }
            return namespaceBuilder.ToString();
        }

        public string Build()
        {
            StringBuilder classBuilder = new StringBuilder();

            string namespaces = BuildUsings(_usedNamespaces);
            classBuilder.Append(namespaces);
            if (namespaces.IsNotEmpty())
            {
                classBuilder.AppendLine();
            }

            classBuilder.AppendLine($"namespace {_classNamespace}");
            classBuilder.AppendLine("{");
            string classBody = BuildClassBody();
            classBuilder.Append(classBody);
            classBuilder.AppendLine("}");

            return classBuilder.ToString();
        }
    }
}