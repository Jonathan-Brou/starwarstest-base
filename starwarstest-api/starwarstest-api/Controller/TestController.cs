using Microsoft.AspNetCore.Mvc;

namespace starwarstest_api.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Service is alive and running!");
        }
    }
}