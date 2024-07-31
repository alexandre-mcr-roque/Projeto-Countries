using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class MapLinks
    {
        /// <summary>
        /// Link to Google Maps
        /// </summary>
        [JsonProperty("googleMaps")]
        public string GoogleMaps { get; set; } = "n/a";

        /// <summary>
        /// Link to OpenStreetMap
        /// </summary>
        [JsonProperty("openStreetMaps")]
        public string OpenStreetMaps { get; set; } = "n/a";
    }
}
