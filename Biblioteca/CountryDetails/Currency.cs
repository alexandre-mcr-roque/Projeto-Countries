using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class Currency
    {
        /// <summary>
        /// The name of the coin
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = "n/a";

        /// <summary>
        /// The symbol of the coin
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = "n/a";
    }
}
