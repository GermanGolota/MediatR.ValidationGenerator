using MediatR.ValidationGenerator.Gen.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Builders
{
    public class MethodBuilder
    {
        private int _leftMargin;

        private bool _isOverride = false;
        private bool _isStatic = false;

        private string _returnType;
        private string _methodName;
        private AccessModifier _modifier = AccessModifier.Public;

        private List<KeyValuePair<string, string>> _parameters = new List<KeyValuePair<string, string>>(); //Type to param name

        private MethodBodyBuilder _body;

        public MethodBuilder()
        {
            _leftMargin = 0;
        }

        public MethodBuilder(int margin)
        {
            _leftMargin = margin;
        }

        public MethodBuilder WithName(string name)
        {
            _methodName = name;

            return this;
        }

        public MethodBuilder WithBody()
        {
            _body = new MethodBodyBuilder(_leftMargin);
            return this;
        }

        public MethodBuilder WithReturnType(string type)
        {
            _returnType = type;

            return this;
        }

        public MethodBuilder WithParameter(string type, string parameterName)
        {
            _parameters.Add(new KeyValuePair<string, string>(type, parameterName));

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

        public MethodBuilder AppendLine(string line)
        {
            _body.AppendLine(line);

            return this;
        }

        public ValueOrNull<string> Build()
        {
            ValueOrNull<string> result;
            if (_methodName.IsEmpty())
            {
                result = ValueOrNull<string>.CreateNull("Can't create method with no name");
            }
            else
            {
                if (_returnType.IsEmpty())
                {
                    result = ValueOrNull<string>.CreateNull("Can't create method with no return type");
                }
                else
                {
                    _body ??= new MethodBodyBuilder();

                    string signature = BuildSignature();
                    string body = _body.Build();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(signature);
                    sb.Append(body);

                    result = sb.ToString();
                }
            }
            return result;
        }

        private string BuildSignature()
        {
            string overrideText = _isOverride ? "override " : "";
            string staticText = _isStatic ? "static " : "";
            StringBuilder signatureBuilder = new StringBuilder();
            signatureBuilder.Repeat("\t", _leftMargin);
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
                parameters.Add($"{parameter.Key} {parameter.Value}");
            }
            string parameterStr = String.Join(", ", parameters);
            return parameterStr;
        }
    }

    public enum AccessModifier
    {
        Public, Private, Protected
    }
}
