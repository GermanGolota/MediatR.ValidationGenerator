using ExampleApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExampleApp.Controllers;

[ApiController]
[Route("api")]
public class SampleController : ControllerBase
{
    private readonly IMediator _mediator;

    public SampleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("duplicate")]
    public async Task<ActionResult<string>> Duplicate([FromBody] DuplicateRequest request)
    {
        var result = await _mediator.Send(request);
        return result;
    }
}
