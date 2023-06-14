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
            return Ok(new {
                message = "Service is alive and running!"
            });
        }
    }
}