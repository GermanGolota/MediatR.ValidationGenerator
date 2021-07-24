using MediatR.ValidationGenerator.Gen.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Builders.Abstractions
{
    public abstract class ValidatingBuilder : IBuilder
    {
        public ValueOrNull<string> Build()
        {
            ValueOrNull<string> result;
            var validationResult = Validate();
            if (validationResult.IsSuccessfull)
            {
                result = BuildInner();
            }
            else
            {
                result = ValueOrNull<string>.CreateNull(validationResult.FailureMessage);
            }
            return result;
        }

        protected abstract string BuildInner();

        public abstract SuccessOrFailure Validate();
    }
}
