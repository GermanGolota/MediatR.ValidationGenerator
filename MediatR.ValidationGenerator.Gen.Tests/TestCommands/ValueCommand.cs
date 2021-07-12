using System;
using System.ComponentModel.DataAnnotations;

namespace MediatR.ValidationGenerator.Gen.Tests.TestCommands
{
    public class ValueCommand : BaseRequest<string>
    {
        [Required]
        public Guid ValueId { get; set; }
    }
}
