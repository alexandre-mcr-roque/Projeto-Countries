using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class Flag
    {
        /// <summary>
        /// The link for the flag's PNG
        /// </summary>
        [JsonProperty("png")]
        public string PNG { get; set; } = "n/a";

        /// <summary>
        /// The link for the flag's SVG
        /// </summary>
        [JsonProperty("svg")]
        public string SVG { get; set; } = "n/a";

        /// <summary>
        /// Descriptive text for the flag
        /// </summary>
        [JsonProperty("alt")]
        public string Alt { get; set; } = "n/a";
    }
}
