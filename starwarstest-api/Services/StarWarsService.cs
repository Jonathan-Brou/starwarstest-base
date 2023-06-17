using Domain.Interfaces;
using Domain.Models;

namespace Services
{
    public class StarWarsService : IStarWarsService
    {
        private readonly ISwapapiClient _swapapiClient;

        public StarWarsService(ISwapapiClient swapapiClient)
        {
            _swapapiClient = swapapiClient;
        }

        public async Task<List<Character>> GetAllAndSortCharactersAsync()
        {
            var characters = await _swapapiClient.GetCharactersFromOriginalTrilogyAsync();

            var sortedCharacters = characters
                .OrderBy(character => character.Films.Min(film => film.EpisodeId)) // Sort by film #
                .ThenBy(character => character.Planet?.Name) // Sort by planet name
                .ThenBy(character => character.Name); // Sort by character name
            return sortedCharacters.ToList();
        }
    }
}