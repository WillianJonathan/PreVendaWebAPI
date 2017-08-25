using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Y to start");

            while (Console.ReadLine().ToUpper().Equals("Y"))
            {
                MainAsync().GetAwaiter().GetResult();
                Console.WriteLine("Do you want to repeat? Y / N");
            }

        }


        public static async Task MainAsync()
        {

            Console.Title = "Client";


            Console.WriteLine("===== AGUARDANDO LIBERAÇÃO PARA COMEÇAR  ======");
            Console.ReadLine();

            var token = await GetTokenAsync();

            if (token != null)
                await CallApi(token);

            Console.WriteLine("===== TERMINEI ======");
            Console.ReadLine();


        }

        private static async Task<string> GetTokenAsync()
        {

            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            if (disco.IsError)
            {
                Console.WriteLine("Disco error {0}", disco.Error);
                return null;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1" );

            if (tokenResponse.IsError)
            {
                Console.WriteLine("Token endpoint error: {0}", tokenResponse.Error);
                return null;
            }

            Console.WriteLine("Success obtaining an access token");

            return tokenResponse.AccessToken;
        }

        private static async Task CallApi(string token)
        {
            // call api
            var client = new HttpClient();
            client.SetBearerToken("");

            try
            {

                var response = await client.GetAsync("http://localhost:5001/api/mesas");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error consuming API {0}", response.StatusCode);

                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(content));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }


    }
}
