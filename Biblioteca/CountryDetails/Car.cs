using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class Car
    {
        /// <summary>
        /// The country's letters in a car's registration
        /// </summary>
        [JsonProperty("signs")]
        public List<string> Signs { get; set; } = [];

        /// <summary>
        /// The front orientation side of the road
        /// </summary>
        [JsonProperty("side")]
        public string Side { get; set; } = "n/a";
    }
}
