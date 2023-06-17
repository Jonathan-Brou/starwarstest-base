using Newtonsoft.Json;

namespace Domain.Models{
public class Planet
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("climate")]
    public string Climate { get; set; }

    [JsonProperty("diameter")]
    public string Diameter { get; set; }

    [JsonProperty("films")]
    public List<string> FilmUrls { get; set; }

    [JsonProperty("gravity")]
    public string Gravity { get; set; }

    [JsonProperty("orbital_period")]
    public string OrbitalPeriod { get; set; }

    [JsonProperty("population")]
    public string Population { get; set; }

    [JsonProperty("residents")]
    public List<string> ResidentUrls { get; set; }

    [JsonProperty("rotation_period")]
    public string RotationPeriod { get; set; }

    [JsonProperty("species")]
    public List<string> SpeciesUrls { get; set; }

    [JsonProperty("surface_water")]
    public string SurfaceWater { get; set; }

    [JsonProperty("terrain")]
    public string Terrain { get; set; }


}
}