using System.Net;

namespace Biblioteca.Services
{
    public class NetworkService
    {

        /// <summary>
        /// With WebClient
        /// </summary>
        public static Response CheckConnection()
        {
            var client = new WebClient();
            try
            {
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    {
                        Success = true
                    };
                }
            }
            catch
            {
                return new Response
                {
                    Success = false,
                    Message = "No internet connection"
                };
            }
        }

        /// <summary>
        /// With HttpClient
        /// </summary>
        /// <returns></returns>
        public static async Task<Response> CheckConnectionHttpClient()
        {
            var client = new HttpClient();
            try
            {
                await client.GetAsync("http://clients3.google.com/generate_204");
                return new Response
                {
                    Success = true
                };
            }
            catch
            {
                return new Response
                {
                    Success = false,
                    Message = "No internet connection"
                };
            }
        }
    }
}
