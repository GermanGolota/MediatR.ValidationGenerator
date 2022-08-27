using ExampleApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Controllers;

[ApiController]
[Route("api/cache")]
public class CacheController : ControllerBase
{
    private readonly ICacheService _cache;

    public CacheController(ICacheService cache)
    {
        _cache = cache;
    }

    [HttpPost("set")]
    public ActionResult Set(string key, string value)
    {
        _cache.Add(key, value);
        return Ok();
    }
}
