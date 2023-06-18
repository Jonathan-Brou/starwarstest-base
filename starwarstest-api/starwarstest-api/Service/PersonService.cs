using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StarWarsApiCSharp;
using System.IO;
using System.Runtime.CompilerServices;

namespace starwarstest_api.Service
{
    public class PersonService : ControllerBase
    {

        private IRepository<Person> _personRepository = new Repository<Person>();
        private IRepository<Planet> _planetRepository = new Repository<Planet>();
        private IMemoryCache _memoryCache;

        private const string _CACHEKEYPERSONS = "CacheCorrectPersons";


        public PersonService(IMemoryCache memoryCache) {
            this._memoryCache = memoryCache;
        }

        public ICollection<Person> GetOrderedData()
        {
            // Get a list of all characters from SWAPI.

            if (this._memoryCache.TryGetValue(_CACHEKEYPERSONS, out ICollection<Person> correctPersons))
            {
                // If it is found in the cache, it cannot be null.
                return correctPersons!;
            }
            else
            {
                correctPersons = this._personRepository.GetEntities(size: 1000)
                    .Where(x => x.Films.Any(y => {
                        // The movie number is the final part of the address path. In case the address is formatted
                        // "/film/1" or "/film/1/" it sanitizes blank spaces.
                        var movieId = ExtractIdFromPath(y);

                        // Had the SWAPI been using chronological movie orders, this would be
                        // how to grab the characters from the first 3 films.
                        // return (movieId >= 4 && movieId <= 6);

                        // Since SWAPI uses 1-3 to denote the "first" star wars movies, we can just do this:
                        return movieId <= 3;
                    }
                )).ToArray();

                // Adjust the naming conventions of the characters received from the API.
                // Get the homeworld names as well for display.
                foreach (var person in correctPersons)
                {
                    person.Name = GetSurnameGivenNameFormat(person.Name);
                    person.Homeworld = GetPlanetName(ExtractIdFromPath(person.Homeworld));
                }

                correctPersons = OrderPersons(correctPersons);

                // Expire cache in 5 minutes.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
                this._memoryCache.Set(_CACHEKEYPERSONS, correctPersons, cacheEntryOptions);
            }
            
            return correctPersons;
        }

        private int ExtractIdFromPath(string path)
        {
            return int.Parse(path.Split("/", StringSplitOptions.RemoveEmptyEntries).Last());
        }

        public static string GetSurnameGivenNameFormat(string name)
        {
            string[] nameParts = name.Split(' ');
            // ex: Jinn - Qui Gon
            if (nameParts.Length > 2)
            {
                return $"{nameParts[nameParts.Length - 1]} - {string.Join(' ', nameParts, 0, nameParts.Length - 1)}";
            }

            // ex: Skywalker - Luke
            else if (nameParts.Length > 1)
            {
                return $"{nameParts[1]} - {nameParts[0]}";
            }

            // ex: Yoda
            return name;
        }

        public string GetPlanetName(int id)
        {
            return this._planetRepository.GetById(id).Name;
        }

        private ICollection<Person> OrderPersons(ICollection<Person> persons)
        {
            return persons
                .OrderBy(x => x.Films.Select(path => ExtractIdFromPath(path)).Min())
                .ThenBy(x => x.Homeworld)
                .ThenBy(x => x.Name)
                .ToList();
        }

    }
}
