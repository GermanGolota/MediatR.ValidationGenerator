using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Requests
{
    public class DuplicateRequest : IRequest<string>
    {
        [Required]
        [RegularExpression("[A-Z,a-z,0-9,-]")]
        public string Text { get; set; }
    }
}
