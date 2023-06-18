using Newtonsoft.Json;

namespace Domain.Models
{
    public class Film
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("episode_id")]
        public int EpisodeId { get; set; }

        [JsonProperty("opening_crawl")]
        public string OpeningCrawl { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("producer")]
        public string Producer { get; set; }

        [JsonProperty("release_date")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("characters")]
        public List<Uri> Characters { get; set; } // Use List<Uri> or List<FilmUrl> if available

        [JsonProperty("planets")]
        public List<Uri> Planets { get; set; } // Use List<Uri> or List<FilmUrl> if available

        [JsonProperty("starships")]
        public List<Uri> Starships { get; set; } // Use List<Uri> or List<FilmUrl> if available

        [JsonProperty("vehicles")]
        public List<Uri> Vehicles { get; set; } // Use List<Uri> or List<FilmUrl> if available

        [JsonProperty("species")]
        public List<Uri> Species { get; set; } // Use List<Uri> or List<FilmUrl> if available

        public Film(string url, string title, int episodeId, string openingCrawl,
                    string director, string producer, DateTime releaseDate,
                    List<Uri> characters, List<Uri> planets, List<Uri> starships,
                    List<Uri> vehicles, List<Uri> species)
        {
            Url = url;
            Title = title;
            EpisodeId = episodeId;
            OpeningCrawl = openingCrawl;
            Director = director;
            Producer = producer;
            ReleaseDate = releaseDate;
            Characters = characters;
            Planets = planets;
            Starships = starships;
            Vehicles = vehicles;
            Species = species;
        }
    }
}