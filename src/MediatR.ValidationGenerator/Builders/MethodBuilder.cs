using MediatR.ValidationGenerator.Builders.Abstractions;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Builders
{
    public interface IMethodNameSelector
    {
        IMethodReturnTypeSelector WithName(string name);
    }

    public interface IMethodReturnTypeSelector
    {
        IMethodBuilder WithReturnType(string type);
    }

    public interface IMethodBuilder : IBuilder
    {
        IMethodBuilder AsOverride();
        IMethodBuilder AsStatic();
        IMethodBuilder WithBody(Func<MethodBodyBuilder, MethodBodyBuilder> builder);
        IMethodBuilder WithModifier(AccessModifier modifier);
        IMethodBuilder WithParameter(string type, string parameterName);
        IMethodBuilder WithParameter(string type, string parameterName, string defaultValue);
    }

    public class MethodBuilder : IMethodBuilder, IMethodReturnTypeSelector, IMethodNameSelector
    {
        private int _leftMargin;

        private bool _isOverride = false;
        private bool _isStatic = false;

        private string _returnType;
        private string _methodName;
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

        public static IMethodNameSelector Create(int margin = 0)
        {
            return new MethodBuilder(margin);
        }

        private MethodBuilder(int margin)
        {
            _leftMargin = margin;
        }

        public IMethodReturnTypeSelector WithName(string name)
        {
            _methodName = name;

            return this;
        }

        public IMethodBuilder WithReturnType(string type)
        {
            _returnType = type;
            return this;
        }

        public IMethodBuilder WithBody(Func<MethodBodyBuilder, MethodBodyBuilder> builder)
        {
            var body = new MethodBodyBuilder(_leftMargin);
            body = builder(body);
            _body = body;
            return this;
        }

        public IMethodBuilder WithParameter(string type, string parameterName)
        {
            _parameters.Add(new MethodParameter(type, parameterName));
            return this;
        }

        public IMethodBuilder WithParameter(string type, string parameterName, string defaultValue)
        {
            _parameters.Add(new MethodParameter(type, parameterName, defaultValue));
            return this;
        }

        public IMethodBuilder AsOverride()
        {
            _isOverride = true;
            return this;
        }

        public IMethodBuilder AsStatic()
        {
            _isStatic = true;
            return this;
        }

        public IMethodBuilder WithModifier(AccessModifier modifier)
        {
            _modifier = modifier;
            return this;
        }

        public string Build()
        {
            string signature = BuildSignature();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(signature);
            var body = GetBody().Build();
            sb.Append(body);

            return sb.ToString();
        }

        private string BuildSignature()
        {
            string overrideText = _isOverride ? "override " : "";
            string staticText = _isStatic ? "static " : "";
            StringBuilder signatureBuilder = new StringBuilder();
            signatureBuilder.Repeat(BuilderUtils.TAB, _leftMargin);
            signatureBuilder.Append($"{_modifier.ToString().ToLower()} {overrideText}{staticText}{_returnType} {_methodName}");
            signatureBuilder.Append("(");
            string parameterStr = BuildParameters();
            signatureBuilder.Append(parameterStr);
            signatureBuilder.Append(")");
            string signature = signatureBuilder.ToString();
            return signature;
        }

        private string BuildParameters()
        {
            List<string> parameters = new List<string>();
            foreach (var parameter in _parameters)
            {
                string parameterStr = $"{parameter.Type} {parameter.Name}";
                if (parameter.DefaultValue.IsNotEmpty())
                {
                    parameterStr += $" = {parameter.DefaultValue}";
                }
                parameters.Add(parameterStr);
            }
            return string.Join(", ", parameters);
        }
    }
}
