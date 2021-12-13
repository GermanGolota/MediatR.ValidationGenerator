using MediatR.ValidationGenerator.Builders.Abstractions;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Builders
{
    public interface IClassConstructorClassNameSelector
    {
        IClassConstructorBuilder WithClassName(string className);
    }

    public interface IClassConstructorBuilder : IBuilder
    {
        IClassConstructorBuilder WithBody(Func<MethodBodyBuilder, MethodBodyBuilder> builder);
        IClassConstructorBuilder WithModifier(AccessModifier modifier);
        IClassConstructorBuilder WithParameter(string parameterType, string parameterName);
    }

    public class ClassConstructorBuilder : IClassConstructorBuilder, IClassConstructorClassNameSelector
    {
        #region Data
        private int _leftMargin;
        private string _className;

        private AccessModifier _modifier = AccessModifier.Public;
        private List<MethodParameter> _parameters = new List<MethodParameter>();

        private MethodBodyBuilder _body;

        private MethodBodyBuilder GetBody()
        {
            if (_body is null)
            {
                _body = new MethodBodyBuilder(_leftMargin);
            }
            return _body;
        }

        #endregion

        public static IClassConstructorClassNameSelector Create(int margin = 0)
        {
            return new ClassConstructorBuilder(margin);
        }

        private ClassConstructorBuilder(int margin)
        {
            _leftMargin = margin;
        }

        #region BuildAccessors
        public IClassConstructorBuilder WithClassName(string className)
        {
            _className = className;
            return this;
        }

        public IClassConstructorBuilder WithModifier(AccessModifier modifier)
        {
            _modifier = modifier;
            return this;
        }

        public IClassConstructorBuilder WithParameter(string parameterType, string parameterName)
        {
            _parameters.Add(new MethodParameter(parameterType, parameterName));
            return this;
        }

        public IClassConstructorBuilder WithBody(Func<MethodBodyBuilder, MethodBodyBuilder> builder)
        {
            var initial = new MethodBodyBuilder(_leftMargin);
            _body = builder(initial);
            return this;
        }
        #endregion

        public string Build()
        {
            StringBuilder constructorBuilder = new StringBuilder();
            string signature = BuildSignature();
            constructorBuilder.Repeat(BuilderUtils.TAB, _leftMargin);
            constructorBuilder.AppendLine(signature);
            var body = GetBody().Build();
            constructorBuilder.Append(body);
            return constructorBuilder.ToString();
        }

        private string BuildSignature()
        {
            string signature = $"{_modifier.ToString().ToLower()} {_className}";
            string parameters = BuilderUtils.BuildParameterList(_parameters);
            return $"{signature}({parameters})";
        }
    }
}
