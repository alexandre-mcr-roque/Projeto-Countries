using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class PostalCode
    {
        /// <summary>
        /// The postal code's format
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; } = "n/a";

        /// <summary>
        /// The postal code's regex
        /// </summary>
        [JsonProperty("regex")]
        public string Regex { get; set; } = "n/a";
    }
}
