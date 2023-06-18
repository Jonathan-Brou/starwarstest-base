using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using CsvHelper;
using System.Globalization;
using System.Text;

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

[HttpGet("test")]
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

[HttpGet("originaltrilogycharacters/sorted/download-csv")]
    public async Task<IActionResult> DownloadCsv()
    {
        var characters = await _starWarsService.GetAllAndSortCharactersAsync();

        using var memoryStream = new MemoryStream();
        using (var writer = new StreamWriter(memoryStream))
        {
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(characters);
        }

        var csvData = Encoding.UTF8.GetString(memoryStream.ToArray());

        return File(new UTF8Encoding().GetBytes(csvData), "text/csv", "characters.csv");
    }
}
}