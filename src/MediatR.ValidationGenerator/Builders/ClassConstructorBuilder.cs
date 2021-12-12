using MediatR.ValidationGenerator.Builders.Abstractions;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Builders
{
    public class ClassConstructorBuilder : ValidatingBuilder
    {
        #region Data
        private int _leftMargin = 0;
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

        public ClassConstructorBuilder()
        {

        }

        public ClassConstructorBuilder(int margin)
        {
            _leftMargin = margin;
        }

        #region BuildAccessors
        public ClassConstructorBuilder WithClassName(string className)
        {
            _className = className;
            return this;
        }

        public ClassConstructorBuilder WithMargin(int margin)
        {
            _leftMargin = margin;
            return this;
        }


        public ClassConstructorBuilder WithModifier(AccessModifier modifier)
        {
            _modifier = modifier;
            return this;
        }

        public ClassConstructorBuilder WithParameter(string parameterType, string parameterName)
        {
            _parameters.Add(new MethodParameter(parameterType, parameterName));
            return this;
        }

        public ClassConstructorBuilder WithBody(Func<MethodBodyBuilder, MethodBodyBuilder> builder)
        {
            var initial = new MethodBodyBuilder(_leftMargin);
            _body = builder(initial);
            return this;
        }
        #endregion

        public override SuccessOrFailure Validate()
        {
            SuccessOrFailure result;
            if (_className.IsEmpty())
            {
                result = SuccessOrFailure.CreateFailure("Cannot create constructor without class name");
            }
            else
            {
                result = true;
            }
            return result;
        }

        protected override string BuildInner()
        {
            StringBuilder constructorBuilder = new StringBuilder();
            string signature = BuildSignature();
            constructorBuilder.Repeat(BuilderUtils.TAB, _leftMargin);
            constructorBuilder.AppendLine(signature);
            var bodyResult = GetBody().Build();
            bodyResult.Resolve(body => constructorBuilder.Append(body));
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
