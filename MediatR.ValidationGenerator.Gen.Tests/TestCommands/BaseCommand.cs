using System;
using System.ComponentModel.DataAnnotations;

namespace MediatR.ValidationGenerator.Gen.Tests.TestCommands
{
    public abstract class BaseRequest<T> : IRequest<T>
    {
        [Required]
        public Guid RequestId { get; set; }
    }
}
