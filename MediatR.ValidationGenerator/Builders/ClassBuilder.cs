﻿using MediatR.ValidationGenerator.Builders.Abstractions;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MediatR.ValidationGenerator.Builders
{
    public class ClassBuilder : ValidatingBuilder
    {
        protected override IEnumerable<ValidatingBuilder> InnerBuilders
        {
            get
            {
                List<ValidatingBuilder> builders = new List<ValidatingBuilder>();
                if (_methods.IsNotNull())
                {
                    builders.AddRange(_methods);
                }
                if (_constructor.IsNotNull())
                {
                    builders.Add(_constructor);
                }
                return builders;
            }
        }

        private List<string> _implementsList = new List<string>();
        private List<string> _usedNamespaces = new List<string>();

        private List<MethodBuilder> _methods = new List<MethodBuilder>();
        private ClassConstructorBuilder _constructor;

        private string _className;
        private AccessModifier _modifier = AccessModifier.Public;
        private string _classNamespace;

        public ClassBuilder WithClassName(string className)
        {
            _className = className;
            return this;
        }

        public ClassBuilder WithAccessModifier(AccessModifier modifier)
        {
            _modifier = modifier;
            return this;
        }

        public ClassBuilder WithNamespace(string classNamespace)
        {
            _classNamespace = classNamespace;
            return this;
        }

        public ClassBuilder WithMethod(Func<MethodBuilder, MethodBuilder> methodBuilder)
        {
            MethodBuilder initial = new MethodBuilder(2);
            var method = methodBuilder(initial);
            _methods.Add(method);
            return this;
        }

        public ClassBuilder WithConstructor(Func<ClassConstructorBuilder, ClassConstructorBuilder> constructorBuilder)
        {
            var initalCtor = new ClassConstructorBuilder(2)
                .WithClassName(_className);
            _constructor = constructorBuilder(initalCtor);
            return this;
        }

        public ClassBuilder UsingNamespace(string usedNamespace)
        {
            _usedNamespaces.Add(usedNamespace);
            return this;
        }

        public ClassBuilder Implementing(string className)
        {
            _implementsList.Add(className);
            return this;
        }

        private string BuildClassBody()
        {
            StringBuilder classBodyBuilder = new StringBuilder();
            string signature = BuildSignature(_modifier, _className, _implementsList);
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
                var buildResult = _constructor.Build();
                if (buildResult.HasValue)
                {
                    result = buildResult.Value;
                }
            }
            return result;
        }

        [Pure]
        private string BuildMethods(List<MethodBuilder> methods)
        {
            StringBuilder classBodyBuilder = new StringBuilder();
            foreach (var method in methods)
            {
                var methodBuildResult = method.Build();
                if (methodBuildResult.HasValue)
                {
                    classBodyBuilder.Append(methodBuildResult.Value);
                }
            }
            return classBodyBuilder.ToString();
        }

        [Pure]
        private string BuildSignature(AccessModifier modifier, string nameOfClass, List<string> implementsList)
        {
            string className = $"{modifier.ToString().ToLower()} class {nameOfClass}";
            string implements = "";
            if (_implementsList.Count > 0)
            {
                implements = $" : {string.Join(",", implementsList)}";
            }
            return $"{className}{implements}";
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

        protected override string BuildInner()
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

        public override SuccessOrFailure Validate()
        {
            SuccessOrFailure result;
            if (_className.IsEmpty())
            {
                result = SuccessOrFailure.CreateFailure("Can't create class without name");
            }
            else
            {
                if (_classNamespace.IsEmpty())
                {
                    result = SuccessOrFailure.CreateFailure("Can't create class in no namespace");
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
    }
}