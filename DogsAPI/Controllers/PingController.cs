using Microsoft.AspNetCore.Mvc;

namespace Dogs.API.Controllers;

[ApiController]
[Route("ping")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok("Dogshouseservice.Version1.0.1");
    }
}
