using ExampleApp.Services;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Requests;

public class DuplicateRequest : IRequest<string>
{
    [Required]
    [RegularExpression("[A-Z,a-z,0-9,-]")]
    public string Text { get; set; }

    [CustomValidator(typeof(ICacheService), nameof(ICacheService.Has))]
    public string CachedValue { get; set; }
}
