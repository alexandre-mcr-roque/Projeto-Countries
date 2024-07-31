using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class Name
    {
        /// <summary>
        /// The official name
        /// </summary>
        [JsonProperty("official")]
        public string Official { get; set; } = "n/a";

        /// <summary>
        /// The common name
        /// </summary>
        [JsonProperty("common")]
        public string Common { get; set; } = "n/a";
    }
}
