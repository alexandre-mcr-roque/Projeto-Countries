using Biblioteca.Services.Reports;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization.Json;

namespace Biblioteca.Services
{
    public class ApiService
    {
        public static string ApiUrl { get; } = "https://restcountries.com";
        public static async Task<Response> GetCountries(CancellationToken ct, IProgress<CountryLoadProgressReport> progress)
        {
            try
            {
                var report = new CountryLoadProgressReport
                {
                    TotalCountries = 100    // Most of the time is spent on the HttpClient's GET request...
                };

                var client = new HttpClient();
                client.BaseAddress = new Uri(ApiUrl);

                string controller = "/v3.1/all";
                var response = await client.GetAsync(controller, ct);
                report.LoadedCountries = 50;
                progress.Report(report);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response()
                    {
                        Success = false,
                        Message = "The website was unreachable"
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                report.LoadedCountries = 80;
                progress.Report(report);

                var countries = JsonConvert.DeserializeObject<List<Country>>(result);
                report.LoadedCountries = 100;
                progress.Report(report);

                return new Response
                {
                    Success = true,
                    Message = countries
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Success = false,
                    Message = "Something wrong happened when loading the data"
                };
            }
        }
    }
}
