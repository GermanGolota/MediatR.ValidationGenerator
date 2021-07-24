﻿using MediatR.ValidationGenerator.Gen.Builders.Abstractions;
using MediatR.ValidationGenerator.Gen.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Builders
{

    public class MethodBuilder : ValidatingBuilder
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

        public MethodBuilder()
        {
            _leftMargin = 0;
        }

        public MethodBuilder(int margin)
        {
            _leftMargin = margin;
        }

        public MethodBuilder WithMargin(int margin)
        {
            _leftMargin = margin;
            return this;
        }

        public MethodBuilder IncreaseMargin(int margin)
        {
            _leftMargin += margin;
            return this;
        }

        public MethodBuilder WithName(string name)
        {
            _methodName = name;

            return this;
        }

        public MethodBuilder WithBody(Func<int, MethodBodyBuilder> builder)
        {
            var body = builder(_leftMargin);
            _body = body;
            return this;
        }

        public MethodBuilder WithReturnType(string type)
        {
            _returnType = type;
            return this;
        }

        public MethodBuilder WithParameter(string type, string parameterName)
        {
            _parameters.Add(new MethodParameter(type, parameterName));
            return this;
        }

        public MethodBuilder WithParameter(string type, string parameterName, string defaultValue)
        {
            _parameters.Add(new MethodParameter(type, parameterName, defaultValue));
            return this;
        }

        public MethodBuilder AsOverride()
        {
            _isOverride = true;
            return this;
        }

        public MethodBuilder AsStatic()
        {
            _isStatic = true;
            return this;
        }

        public MethodBuilder WithModifier(AccessModifier modifier)
        {
            _modifier = modifier;
            return this;
        }

        public override SuccessOrFailure Validate()
        {
            SuccessOrFailure result;
            if (_methodName.IsEmpty())
            {
                result = SuccessOrFailure.CreateFailure("Can't create method with no name");
            }
            else
            {
                if (_returnType.IsEmpty())
                {
                    result = SuccessOrFailure.CreateFailure("Can't create method with no return type");
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        protected override string BuildInner()
        {
            string signature = BuildSignature();
            var body = GetBody().Build();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(signature);
            if (body.HasValue)
            {
                sb.Append(body.Value);
            }

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
            string parameterStr = BuilderUtils.BuildParameterList(_parameters);
            signatureBuilder.Append(parameterStr);
            signatureBuilder.Append(")");
            string signature = signatureBuilder.ToString();
            return signature;
        }
    }
}
