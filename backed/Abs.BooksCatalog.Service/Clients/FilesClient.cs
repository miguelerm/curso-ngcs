using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Abs.BooksCatalog.Service.Clients
{
    public class FilesClient
    {
        private readonly HttpClient http;

        public FilesClient(HttpClient http)
        {
            this.http = http;
        }

        public async Task Put(string code)
        {
            var response = await http.PutAsync($"https://localhost:5003/api/files/{code}", new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
    }
}
