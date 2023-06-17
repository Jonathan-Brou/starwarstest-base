using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
namespace starwarstest_api.controllers{

[ApiController]
[Route("/")]
public class StarWarsController : ControllerBase
{
    private readonly IStarWarsService _starWarsService;

    public StarWarsController(IStarWarsService starWarsService)
    {
        _starWarsService = starWarsService;
    }

[HttpGet]
public async Task<IActionResult> GetCharacters()
{
    try
    {
        var characters = await _starWarsService.GetAllAndSortCharactersAsync();
        return Ok(characters); //get 200 request
    }
    catch (Exception ex)
    {
        //this is hwere i would probably log the message as well.
        // Return a generic error message to the client. I will not expose anything more than needed here.
        return StatusCode(500, "An error occurred while processing your request. Please try again later.");
    }
}
}
}