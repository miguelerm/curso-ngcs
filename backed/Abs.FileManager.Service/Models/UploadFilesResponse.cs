using System.Collections.Generic;

namespace Abs.FileManager.Service.Models
{
    public class UploadFilesResponse
    {
        public long TotalSize { get; set; }
        public IDictionary<string, string> Files { get; set; }
    }
}
