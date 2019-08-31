using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Abs.FilesManager.Services.Models
{
    public class UploadFileRequest
    {
        public string Tag { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
