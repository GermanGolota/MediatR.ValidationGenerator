using MediatR.ValidationGenerator.Builders.Abstractions;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        public static IClassConstructorClassNameSelector Create(int margin = 0)
        {
            return new ClassConstructorBuilder(margin);
        }

        private ClassConstructorBuilder(int margin)
        {
            _leftMargin = margin;
        }

        #region DataFields
        //Required
        private string _className = null!;
        //Optional
        private MethodBodyBuilder? _body = null;
        private MethodBodyBuilder GetBody()
        {
            if (_body is null)
            {
                _body = new MethodBodyBuilder(_leftMargin);
            }
            return _body;
        }

        private int _leftMargin;
        private AccessModifier _modifier = AccessModifier.Public;
        private List<MethodParameter> _parameters = new List<MethodParameter>();
        #endregion
        #region AccessMethods
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
        #region Build
        public string Build()
        {
            StringBuilder constructorBuilder = new StringBuilder();
            string signature = BuildSignature(_modifier, _className, _parameters);
            constructorBuilder.Repeat(BuilderUtils.TAB, _leftMargin);
            constructorBuilder.AppendLine(signature);
            var body = GetBody().Build();
            constructorBuilder.Append(body);
            return constructorBuilder.ToString();
        }

        [Pure]
        private static string BuildSignature(AccessModifier modifier, string className, List<MethodParameter> parametersList)
        {
            string signature = $"{modifier.ToString().ToLower()} {className}";
            string parameters = BuilderUtils.BuildParameterList(parametersList);
            return $"{signature}({parameters})";
        }
        #endregion
    }
}
