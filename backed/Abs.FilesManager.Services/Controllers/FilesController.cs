using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abs.FilesManager.Services.Models;
using Abs.Messages.BooksCatalog.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Abs.FilesManager.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        

        private static readonly Regex fileNameFormat = new Regex("^[0-9]{14}-[0-9a-fA-F]{32}$");
        private readonly IRequestClient<IGetBookByIdRequest> client;

        public FilesController(IRequestClient<IGetBookByIdRequest> client)
        {
            this.client = client;
        }

        [HttpGet("test")]
        public async Task<ActionResult<IGetBookByIdResponse>> GetTest()
        {
            var book = await client.GetResponse<IGetBookByIdResponse>(new { id = 1 });
            return Ok(book);
        }

        [HttpPost]
        public ActionResult Post([FromForm] UploadFileRequest model)
        {
            var folder = Path.GetTempPath();
            var totalSize = 0L;
            var resultFiles = new Dictionary<string, string>(model.Files.Count());

            foreach (var file in model.Files)
            {
                if (file.Length <= 0)
                {
                    continue;
                }

                var code = $"{DateTime.UtcNow:yyyyMMddhhmmss}-{Guid.NewGuid():N}";
                var (path, meta) = BuildFilePaths(folder, code);

                SaveFile(file, path);
                SaveMetadataFile(meta, new FileMetadata
                {
                    ContentType = file.ContentType,
                    FileName = file.FileName,
                    Length = file.Length,
                    Tag = model.Tag
                });

                resultFiles.Add(code, file.FileName);
                totalSize += file.Length;
            }


            return Ok(new UploadFileResponse {
                TotalSize = totalSize,
                Files = resultFiles
            });
        }

        [HttpGet("{code}")]
        public ActionResult Get(string code, bool download = false)
        {
            if (!fileNameFormat.IsMatch(code))
            {
                return BadRequest();
            }

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            var (path, meta) = BuildFilePaths(folder, code);

            var file = new FileInfo(path);
            if (!file.Exists)
            {
                return NotFound();
            }

            var metaFile = new FileInfo(meta);
            if (!metaFile.Exists)
            {
                return NotFound();
            }

            var metadata = GetMetadata(metaFile);
            var downloadName = metadata.FileName;
            var contentType = metadata.ContentType;
            var stream = file.OpenRead();

            if (download)
            {
                return File(stream, contentType, downloadName);
            }
            else
            {
                return File(stream, contentType);
            }
        }

        private static FileMetadata GetMetadata(FileInfo file)
        {
            using (var stream = file.OpenText())
            using (var reader = new JsonTextReader(stream))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<FileMetadata>(reader);
            }
        }

        private static (string path, string meta) BuildFilePaths(string folder, string code)
        {
            var path = Path.Combine(folder, $"{code}.file");
            var meta = Path.Combine(folder, $"{code}.meta");
            return (path, meta);
        }

        private static void SaveMetadataFile(string path, FileMetadata metadata)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, metadata);
            }
        }

        private static void SaveFile(IFormFile file, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

    }
}