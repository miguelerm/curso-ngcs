using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
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

            Console.WriteLine("Uploading file...");
            
            var result = await UploadFile(filePath, accessToken);

            if (result == null)
            {
                return;
            }
            
            Console.WriteLine("File uploaded, press enter to download");
            Console.ReadLine();

            Console.WriteLine("Downloading file...");
            var fileId = result.FileId;
            var finalDirectory = Path.GetTempPath();
            await DownloadFile(fileId, finalDirectory, accessToken);

            Console.WriteLine("File {0} saved on {1}", fileId, finalDirectory);
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

        private static async Task<FileUploadResult> UploadFile(string filePath, string accessToken)
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
                {
                    new StreamContent(File.OpenRead(filePath)) 
                    { 
                        Headers = 
                        { 
                            ContentType = MediaTypeHeaderValue.Parse("image/jpeg") 
                        } 
                    }, 
                    "File", 
                    fileName
                }
            };

            using var response = await apiClient.PostAsync("/api/files", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Api Error {0}: {1}", response.StatusCode, errorMessage);
                return null;
            }
            else
            {
                var result = await response.Content.ReadAsAsync<FileUploadResult>();
                Console.WriteLine("Api Response File Id: {0}", result.FileId);
                return result;
            }
        }

        private static async Task DownloadFile(Guid fileId, string destinationPath, string accessToken)
        {
            var apiClient = new HttpClient
            {
                BaseAddress = new Uri(filesApiServer)
            };
            apiClient.SetBearerToken(accessToken);

            using var response = await apiClient.GetAsync($"/api/files/{fileId}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Api Error {0}: {1}", response.StatusCode, errorMessage);
            }
            else
            {
                var contentDisposition = response.Content.Headers.ContentDisposition;
                var originalFileName = contentDisposition.FileName;
                var extension = Path.GetExtension(originalFileName);

                Console.WriteLine("Original file name: {0}", originalFileName);

                using var stream = await response.Content.ReadAsStreamAsync();
                using var file = File.Create(Path.Combine(destinationPath, $"{fileId}{extension}"));
                await stream.CopyToAsync(file);
            }
        }

        public class FileUploadResult
        {
            public Guid FileId { get; set; }
        }

    }
}
