using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Api.Models
{
    public class UploadFileRequest
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string Container { get; set; }
        [StringLength(50)]
        public string Project { get; set; }
        [StringLength(10)]
        public string Module { get; set; }
        [StringLength(500)]
        public string Reference { get; set; }
    }
}
