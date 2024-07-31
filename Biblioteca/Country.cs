using Biblioteca.CountryDetails;
using Newtonsoft.Json;

namespace Biblioteca
{
    public class Country
    {
        /// <summary>
        /// The country's name
        /// </summary>
        [JsonProperty("name")]
        public CountryName Name { get; protected set; } = new();

        /// <summary>
        /// The TLDs (top-level domains) of the country
        /// </summary>
        [JsonProperty("tld")]
        public List<string> TLD { get; set; } = [];

        /// <summary>
        /// The country's 2-letter code (ISO 3166-1 alpha-2)
        /// </summary>
        [JsonProperty("cca2")]
        public string CCA2 { get; set; } = "n/a";

        /// <summary>
        /// The country's 3-number code (ISO 3166-1 numeric-3)
        /// </summary>
        [JsonProperty("ccn3")]
        public string CCN3 { get; set; } = "n/a";

        /// <summary>
        /// The country's 3-letter code (ISO 3166-1 alpha-3)
        /// </summary>
        [JsonProperty("cca3")]
        public string CCA3 { get; set; } = "n/a";

        /// <summary>
        /// Código do país para os jogos olímpicos
        /// </summary>
        [JsonProperty("cioc")]
        public string CIOC { get; set; } = "n/a";

        /// <summary>
        /// The country's independence
        /// </summary>
        [JsonProperty("independent")]
        public bool? Independent { get; set; } = null;

        /// <summary>
        /// If the country is officially assigned in the ISO 3166-1
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; } = "n/a";

        /// <summary>
        /// The country's UN Member status
        /// </summary>
        [JsonProperty("unMember")]
        public bool? UNMember { get; set; } = null;

        /// <summary>
        /// The currencies used within the country
        /// </summary>
        [JsonProperty("currencies")]
        public Dictionary<string, Currency> Currencies { get; set; } = [];

        /// <summary>
        /// The phone number identifiers
        /// </summary>
        [JsonProperty("idd")]
        public PhoneID IDD { get; set; } = new();

        /// <summary>
        /// The capital(s) of the country
        /// </summary>
        [JsonProperty("capital")]
        public List<string> Capital { get; set; } = [];

        /// <summary>
        /// The alternative spellings for the country's name
        /// </summary>
        [JsonProperty("altSpellings")]
        public List<string> AltSpellings { get; set; } = [];

        /// <summary>
        /// The country's region
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; } = "n/a";

        /// <summary>
        /// The country's subregion
        /// </summary>
        [JsonProperty("subregion")]
        public string Subregion { get; set; } = "n/a";

        /// <summary>
        /// The languages used within the country
        /// </summary>
        [JsonProperty("languages")]
        public Dictionary<string, string> Languages { get; set; } = [];

        /// <summary>
        /// The translations for the country's name
        /// </summary>
        [JsonProperty("translations")]
        public Dictionary<string, Name> Translations { get; set; } = new();

        /// <summary>
        /// The latitude [0] and longitude [1] of the country ([0,0] if the information is not available)
        /// </summary>
        [JsonProperty("latlng")]
        public double[] LatLng { get; set; } = [0, 0];

        /// <summary>
        /// If the country is fully surrounded by land
        /// </summary>
        [JsonProperty("landlocked")]
        public bool? Landlocked { get; set; } = null;

        /// <summary>
        /// The country codes of all the bordering countries
        /// </summary>
        [JsonProperty("borders")]
        public List<string> Borders { get; set; } = [];

        /// <summary>
        /// The country's area in km (-1 if the information is not available)
        /// </summary>
        [JsonProperty("area")]
        public double Area { get; set; } = -1;

        /// <summary>
        /// The demonyms used to refer locals in the country
        /// </summary>
        [JsonProperty("demonyms")]
        public Dictionary<string, Demonym> Demonyms { get; set; } = [];

        /// <summary>
        /// The flag emoji
        /// </summary>
        [JsonProperty("flag")]
        public string Flag { get; set; } = "n/a";

        /// <summary>
        /// The links for the country's location
        /// </summary>
        [JsonProperty("maps")]
        public MapLinks Maps { get; set; } = new();

        /// <summary>
        /// The country's population (-1 if the information is not available)
        /// </summary>
        [JsonProperty("population")]
        public int Population { get; set; } = -1;

        /// <summary>
        /// Worldbank Gini value for the country
        /// (although a Dictionary is used, there is only 1 Gini per country)
        /// </summary>
        [JsonProperty("gini")]
        public Dictionary<string, double> Gini { get; set; } = [];

        /// <summary>
        /// Country's code for FIFA games
        /// </summary>
        [JsonProperty("fifa")]
        public string FIFA { get; set; } = "n/a";

        /// <summary>
        /// Driving-specific characteristics in the country
        /// </summary>
        [JsonProperty("car")]
        public Car Car { get; set; } = new();

        /// <summary>
        /// The country's timezone(s)
        /// </summary>
        [JsonProperty("timezones")]
        public List<string> Timezones { get; set; } = [];

        /// <summary>
        /// The country's continent(s)
        /// </summary>
        [JsonProperty("continents")]
        public List<string> Continents { get; set; } = [];

        /// <summary>
        /// The country's flag (in PNG and SVG)
        /// </summary>
        [JsonProperty("flags")]
        public Flag Flags { get; set; } = new();

        /// <summary>
        /// The country's coat of arms (in PNG and SVG)
        /// </summary>
        [JsonProperty("coatOfArms")]
        public CoatOfArms CoatOfArms { get; set; } = new();

        /// <summary>
        /// The first work day of the week
        /// </summary>
        [JsonProperty("startOfWeek")]
        public string StartOfWeek { get; set; } = "n/a";

        /// <summary>
        /// Information about the country's capital
        /// </summary>
        [JsonProperty("capitalInfo")]
        public CapitalInfo CapitalInfo { get; set; } = new();

        /// <summary>
        /// The format and regex of the country's postal code
        /// </summary>
        [JsonProperty("postalCode")]
        public PostalCode PostalCode { get; set; } = new();

        /// <summary>
        /// Returns the common name of the country with the country's emoji (if it has one)
        /// </summary>
        public override string ToString()
        {
            if (Flag != "n/a")
                return  $"{Name.Common} {Flag}";

            return Name.Common;
        }
    }
}