using Newtonsoft.Json;


namespace Domain.Models
{
    public class Character
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birth_year")]
        public string? BirthYear { get; set; }

        [JsonProperty("eye_color")]
        public string? EyeColor { get; set; }

        [JsonProperty("gender")]
        public string? Gender { get; set; }

        [JsonProperty("hair_color")]
        public string? HairColor { get; set; }

        [JsonProperty("height")]
        public string? Height { get; set; }

        [JsonProperty("mass")]
        public string? Mass { get; set; }

        [JsonProperty("skin_color")]
        public string? SkinColor { get; set; }

        [JsonProperty("homeworld")]
        public string? Homeworld { get; set; }

        [JsonProperty("films")]
        public List<string>? FilmUrls { get; set; }

        [JsonProperty("species")]
        public string[]? Species { get; set; }

        [JsonProperty("starships")]
        public string[]? Starships { get; set; }

        [JsonProperty("vehicles")]
        public string[]? Vehicles { get; set; }

        [JsonProperty("created")]
        public string? Created { get; set; }

        [JsonProperty("edited")]
        public string? Edited { get; set; }

        [JsonIgnore] // We don't want to serialize these fields when sending data to the API
        public List<Film> Films { get; set; } = new List<Film>();

        [JsonIgnore] // We don't want to serialize these fields when sending data to the API
        public Planet Planet { get; set; }




        public void RearrangeName()
        {
            var names = Name.Split(' ');
            var givenName = names[0]; // Assume the first part is the given name
            var surname = names.Length > 1 ? names[1] : null; // Assign the second part as the surname if available

            if (!string.IsNullOrEmpty(surname))
            {
                Name = $"{surname}, {givenName}".TrimEnd(',', ' ');
            }
            else
            {
                Name = givenName; // Use the given name as it is
            }
        }
    }
}