using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MediatR.ValidationGenerator
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T data);
    }

    public class ValidationResult
    {
        //
        // Summary:
        //     Whether validation succeeded
        public virtual bool IsValid { get; }
        //
        // Summary:
        //     A collection of errors
        public List<ValidationFailure> Errors { get; }
    }

    public class ValidationFailure
    {
        //
        // Summary:
        //     The name of the property.
        public string PropertyName { get; set; }
        //
        // Summary:
        //     The error message
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// An exception that represents failed validation
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Validation errors
        /// </summary>
        public IEnumerable<ValidationFailure> Errors { get; private set; }

        /// <summary>
        /// Creates a new ValidationException
        /// </summary>
        /// <param name="errors"></param>
        public ValidationException(IEnumerable<ValidationFailure> errors) : base(BuildErrorMessage(errors))
        {
            Errors = errors;
        }

        private static string BuildErrorMessage(IEnumerable<ValidationFailure> errors)
        {
            var arr = errors.Select(x => $"{Environment.NewLine} -- {x.PropertyName}: {x.ErrorMessage}");
            return "Validation failed: " + string.Join(string.Empty, arr);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");

            info.AddValue("errors", Errors);
            base.GetObjectData(info, context);
        }
    }

}
