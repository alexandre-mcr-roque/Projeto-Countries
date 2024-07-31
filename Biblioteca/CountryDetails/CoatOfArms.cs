using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class CoatOfArms
    {
        /// <summary>
        /// The link for the coat of arms' PNG
        /// </summary>
        [JsonProperty("png")]
        public string PNG { get; set; } = "n/a";

        /// <summary>
        /// The link for the coat of arms' SVG
        /// </summary>
        [JsonProperty("svg")]
        public string SVG { get; set; } = "n/a";
    }
}
