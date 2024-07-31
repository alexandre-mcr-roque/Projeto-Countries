using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class Demonym
    {
        /// <summary>
        /// The female demonym
        /// </summary>
        [JsonProperty("f")]
        public string F { get; set; } = "n/a";

        /// <summary>
        /// The male demonym
        /// </summary>
        [JsonProperty("m")]
        public string M { get; set; } = "n/a";
    }
}
