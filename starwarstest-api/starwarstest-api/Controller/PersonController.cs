using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarWarsApiCSharp;
using starwarstest_api.Service;

namespace starwarstest_api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {

        private PersonService _personService;

        public PersonController(PersonService personService)
        {
            this._personService = personService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this._personService.GetOrderedData());
        }
    }
}
