using MediatR.ValidationGenerator.Gen.Extensions;
using MediatR.ValidationGenerator.Gen.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Builders.Abstractions
{
    public abstract class ValidatingBuilder : IBuilder
    {
        protected virtual IEnumerable<ValidatingBuilder> InnerBuilders { get; }
        public ValueOrNull<string> Build()
        {
            ValueOrNull<string> result;
            var validationResult = Validate();
            if (validationResult.IsSuccessfull && ValidInnerBuilders())
            {

                result = BuildInner();
            }
            else
            {
                result = ValueOrNull<string>.CreateNull(validationResult.FailureMessage);
            }
            return result;
        }

        private bool ValidInnerBuilders()
        {
            bool result;
            if (InnerBuilders.IsNotNull())
            {
                result = InnerBuilders.None(x => x.Validate().IsFailure);
            }
            else
            {
                result = true;
            }
            return result;
        }

        protected abstract string BuildInner();

        public abstract SuccessOrFailure Validate();
    }
}
