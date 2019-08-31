using System.Collections;
using System.Collections.Generic;

namespace Abs.FilesManager.Services.Models
{
    public class UploadFileResponse
    {
        public long TotalSize { get; set; }
        public IDictionary<string, string> Files { get; set; }
    }
}
