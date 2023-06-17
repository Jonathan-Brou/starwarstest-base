using Domain.Models;
using Newtonsoft.Json;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace DataAccess.Clients
{
    public class SwapapiClient : ISwapapiClient
    {
        // Fields for HTTP client and memory cache
        private readonly HttpClient _httpClient;
        private IMemoryCache _cache;

        // Constructor with injected HTTP client factory and memory cache
        public SwapapiClient(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            // Create an HTTP client from the factory
            _httpClient = httpClientFactory.CreateClient();
            // Set the cache field
            _cache = cache;
        }

        public async Task<IEnumerable<Character>> GetCharactersFromOriginalTrilogyAsync()
        {
            // Hard-coded IDs for the original trilogy films
            var originalTrilogyIds = new[] { 1, 2, 3 };

            // Character dictionary to store character data
            var characters = new Dictionary<string, Character>();

            // Create tasks for each film in the original trilogy
            var filmTasks = originalTrilogyIds.Select(async filmId =>
            {
                // Check if the film data is already in the cache
                if (!_cache.TryGetValue($"film_{filmId}", out Film film))
                {
                    // If not, fetch film data from the API
                    var filmResponse = await _httpClient.GetStringAsync($"https://swapi.dev/api/films/{filmId}/");
                    film = JsonConvert.DeserializeObject<Film>(filmResponse);

                    // Cache the film data for 24 hours
                    _cache.Set($"film_{filmId}", film, TimeSpan.FromHours(24));
                }

                // Create tasks for each character in the film
                var characterTasks = film.Characters.Select(async characterUrl =>
                {
                    var characterUriStr = characterUrl.ToString();

                    // Check if the character data is already in the cache
                    if (!_cache.TryGetValue($"character_{characterUriStr}", out Character character))
                    {
                        // If not, fetch character data from the API
                        var characterResponse = await _httpClient.GetStringAsync(characterUriStr);
                        character = JsonConvert.DeserializeObject<Character>(characterResponse);
                                // Rearrange name to 'surname, given name' format
                                character.RearrangeName();


                        // Check if the homeworld data is already in the cache
                        if (!_cache.TryGetValue($"homeworld_{character.Homeworld}", out Planet homeworld))
                        {
                            // If not, fetch homeworld data from the API
                            var homeworldResponse = await _httpClient.GetStringAsync(character.Homeworld);
                            homeworld = JsonConvert.DeserializeObject<Planet>(homeworldResponse);

                            // Cache the homeworld data for 24 hours
                            _cache.Set($"homeworld_{character.Homeworld}", homeworld, TimeSpan.FromHours(24));
                        }

                        // Assign homeworld to the character
                        character.Planet = homeworld;
                        // Assign the film to the character's films list
                        character.Films = new List<Film> { film };

                        // Cache the character data for 24 hours
                        _cache.Set($"character_{characterUriStr}", character, TimeSpan.FromHours(24));
                    }

                    // Return the character
                    return character;
                });

                // Run character tasks in parallel and wait for them to complete
                var charactersInFilm = await Task.WhenAll(characterTasks);

                // Iterate over characters in the film
                foreach (var character in charactersInFilm)
                {
                    // If the character is already in the dictionary, add the film to its list of films
                    if (characters.ContainsKey(character.Name))
                    {
                        characters[character.Name].Films.Add(film);
                    }
                    else // If the character is not in the dictionary, add it
                    {
                        characters[character.Name] = character;
                    }
                }
            });

            // Send film requests in parallel
            await Task.WhenAll(filmTasks);

            return characters.Values;
        }
    }
}