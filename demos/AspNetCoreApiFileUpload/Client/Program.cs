using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Client
{
    public class Program
    {
        private const string authenticationServer = "http://localhost:5000";
        private const string filesApiServer = "http://localhost:5001";
        private static async Task Main(string[] args)
        {

            var tokenResponse = await GetAccessToken();

            if (tokenResponse is null)
            {
                return;
            }

            if (tokenResponse.IsError)
            {
                Console.WriteLine("Token Error: {0}", tokenResponse.Error);
                return;
            }

            Console.WriteLine("Using token: {0}", tokenResponse.Json);
            Console.WriteLine("\n\n");
            
            var accessToken = tokenResponse.AccessToken;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "borbotones.jpg");

            await UploadFile(filePath, accessToken);

            Console.ReadLine();
        }

        private static async Task<TokenResponse> GetAccessToken()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(authenticationServer)
            };

            var disco = await client.GetDiscoveryDocumentAsync();
            if (disco.IsError)
            {
                Console.WriteLine("discovery error: {0}", disco.Error);
                return null;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "test:console",
                ClientSecret = "test:client:secret",

                Scope = "files:api"
            });

            return tokenResponse;
        }

        private static async Task UploadFile(string filePath, string accessToken)
        {
            var fileName = Path.GetFileName(filePath);
            // call api
            var apiClient = new HttpClient
            {
                BaseAddress = new Uri(filesApiServer)
            };
            apiClient.SetBearerToken(accessToken);
            using var content = new MultipartFormDataContent
            {
                {new StringContent("mi contenedor"), "Container"},
                {new StringContent("tu proyecto"), "Project"},
                {new StringContent("su módulo"), "Module"},
                {new StringContent("nuestra referencia"), "Reference"},
                {new StreamContent(File.OpenRead(filePath)), "File", fileName}
            };

            using var response = await apiClient.PostAsync("/api/files", content);
            if (!response.IsSuccessStatusCode)
            {
                var t = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Api Error {0}", response.StatusCode);
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Api Response {0}", responseString);
            }
        }
    }
}
