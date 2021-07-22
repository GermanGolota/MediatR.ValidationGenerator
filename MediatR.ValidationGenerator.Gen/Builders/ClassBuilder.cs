﻿using MediatR.ValidationGenerator.Gen.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Builders
{
    public class ClassBuilder
    {
        private List<string> _implementsList = new List<string>();
        private List<string> _usedNamespaces = new List<string>();

        private List<MethodBuilder> _methods = new List<MethodBuilder>();

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

        public ClassBuilder WithMethod(MethodBuilder method)
        {
            method.IncreaseMargin(2);
            _methods.Add(method);
            return this;
        }

        public ClassBuilder WithMethod(Func<MethodBuilder> methodBuilder)
        {
            return WithMethod(methodBuilder());
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

        public string Build()
        {
            StringBuilder classBuilder = new StringBuilder();

            string namespaces = BuildUsings(_usedNamespaces);
            classBuilder.Append(namespaces);

            classBuilder.AppendLine();

            classBuilder.AppendLine($"namespace {_classNamespace}");
            classBuilder.AppendLine("{");
            string classBody = BuildClassBody();
            classBuilder.Append(classBody);
            classBuilder.AppendLine("}");

            return classBuilder.ToString();
        }

        private string BuildClassBody()
        {
            StringBuilder classBodyBuilder = new StringBuilder();
            string signature = BuildSignature(_modifier, _className, _implementsList);
            classBodyBuilder.AppendLine($"{BuilderConstants.TAB}{signature}");
            classBodyBuilder.AppendLine(BuilderConstants.TAB + "{");
            string methods = BuildMethods(_methods);
            classBodyBuilder.AppendLine(methods);
            classBodyBuilder.AppendLine(BuilderConstants.TAB + "}");
            return classBodyBuilder.ToString();
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
                    classBodyBuilder.AppendLine(methodBuildResult.Value);
                    classBodyBuilder.AppendLine();
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
                implements = $" : {String.Join(",", implementsList)}";
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
    }
}