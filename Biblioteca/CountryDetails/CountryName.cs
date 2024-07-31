using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class CountryName
    {
        /// <summary>
        /// The country's common name
        /// </summary>
        [JsonProperty("common")]
        public string Common { get; set; } = "n/a";

        /// <summary>
        /// The country's official name
        /// </summary>
        [JsonProperty("official")]
        public string Official { get; set; } = "n/a";

        /// <summary>
        /// The country's native names
        /// </summary>
        [JsonProperty("nativeName")]
        public Dictionary<string, Name> NativeName { get; set; } = [];
    }
}
