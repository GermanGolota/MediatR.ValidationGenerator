﻿using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using System.Collections.Generic;

namespace MediatR.ValidationGenerator.Builders.Abstractions
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