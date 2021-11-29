using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Requests
{
    public class DuplicateRequest : IRequest<string>
    {
        [Required]
        public string Text { get; set; }
    }
}
