using System.Collections.Generic;

namespace Abs.FileManager.Service.Models
{
    public class UpdateFilesResponse
    {
        public long TotalSize { get; set; }
        public IEnumerable<string> Files { get; set; }
    }
}
