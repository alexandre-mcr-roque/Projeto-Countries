using Newtonsoft.Json;

namespace Biblioteca.CountryDetails
{
    /// <summary>
    /// A phone's identifier.<br/>
    /// They contain a root and 0 or more suffixes.
    /// By joining a root and a suffix you get a valid identifier
    /// </summary>
    public class PhoneID
    {
        /// <summary>
        /// The identifier's common string (e.g. '+3' for Portugal)
        /// </summary>
        [JsonProperty("root")]
        public string Root { get; set; } = "n/a";

        /// <summary>
        /// The identifier's different suffixes (e.g. '51' for Portugal)
        /// </summary>
        [JsonProperty("suffixes")]
        public List<string> Suffixes { get; set; } = [];
    }
}
