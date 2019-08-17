using System.Collections.Generic;

namespace Abs.FileManager.Service.Models
{
    public class UpdateFilesRequest
    {
        public string Tag { get; set; }
        public IEnumerable<string> Files { get; set; }
    }
}
