using System;
using System.IO;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string directory;

        public FilesController()
        {
            directory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"{User.FindFirst("client_id")} Everything Ok";
        }

        [HttpPost]
        public async Task<ActionResult<UploadFileResponse>> PostAsync([FromForm] UploadFileRequest request)
        {
            var fileId = Guid.NewGuid();

            var finalFileName = Path.Combine(directory, $"{fileId}.file");
            var finalMedataFileName = Path.Combine(directory, $"{fileId}.json");

            var postedFile = request.File;

            using (var file = postedFile.OpenReadStream())
            using (var fileData = System.IO.File.Create(finalFileName))
            {
                await file.CopyToAsync(fileData);
            }

            using (var metadataFile = System.IO.File.CreateText(finalMedataFileName))
            {
                var metadata = new MetadataFile {
                    FileName = postedFile.FileName,
                    ContentType = postedFile.ContentType,
                    Length = postedFile.Length
                };
                var serializer = new JsonSerializer();
                serializer.Serialize(metadataFile, metadata);
            }

            return new UploadFileResponse { 
                FileId = fileId
            };
        }
        [HttpGet("{fileId:guid}")]
        public ActionResult Get([FromRoute] Guid fileId)
        {
            var finalFileName = Path.Combine(directory, $"{fileId}.file");
            var finalMedataFileName = Path.Combine(directory, $"{fileId}.json");

            if (!System.IO.File.Exists(finalMedataFileName))
            {
                return NotFound();
            }

            if (!System.IO.File.Exists(finalFileName))
            {
                return NotFound();
            }

            MetadataFile metadata;
            using (var metadataFile = System.IO.File.OpenText(finalMedataFileName))
            using (var reader = new JsonTextReader(metadataFile))
            {
                var serializer = new JsonSerializer();
                metadata = serializer.Deserialize<MetadataFile>(reader);
            }

            var contentType = metadata?.ContentType ?? "application/octet-stream";
            var fileName = metadata?.FileName ?? fileId.ToString();
            var file = System.IO.File.OpenRead(finalFileName);
            return File(file, contentType, fileName);
        }
    }

    public class MetadataFile
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
    }

}