using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Abs.FileManager.Service.Models
{
    public class UploadFilesModel
    {
        public string Tag { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
