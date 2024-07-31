using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    public class CapitalInfo
    {
        /// <summary>
        /// The latitude [0] and longitude [1] of the capital ([0,0] if the information is not available)
        /// </summary>
        [JsonProperty("latlng")]
        public double[] LatLng { get; set; } = [0, 0];
    }
}
