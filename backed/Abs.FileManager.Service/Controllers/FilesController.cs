using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abs.FileManager.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Abs.FileManager.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private static readonly Regex nameFormat = new Regex("[0-9]{8}T[0-9]{6}-[0-9a-fA-F]{32}");
        protected virtual string TemporaryPath => Path.GetTempPath();
        protected virtual string FinalPath => Path.Combine(Directory.GetCurrentDirectory(), "Files");


        [HttpPost]
        public async Task<ActionResult<UploadFilesResponse>> PostAsync([FromForm] UploadFilesModel model)
        {
            long size = model.Files.Sum(x => x.Length);
            var fileNames = new Dictionary<string, string>(model.Files.Count());
            
            foreach (var file in model.Files)
            {
                if (file.Length <= 0)
                {
                    continue;
                }

                var name = $"{DateTime.UtcNow:yyyyMMddTHHmmss}-{Guid.NewGuid():N}";
                var (path, meta) = BuildFileName(TemporaryPath, name);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                using (var stream = new FileStream(meta, FileMode.Create))
                using (var writer = new StreamWriter(stream))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, new FileMetadata {
                        ContentType = file.ContentType,
                        FileName = file.FileName,
                        Length = file.Length,
                        Tag = model.Tag
                    });
                }

                fileNames.Add(name, file.FileName);
            }

            return Ok(new UploadFilesResponse { TotalSize = size, Files = fileNames });
        }

        [HttpPut]
        public ActionResult<UpdateFilesResponse> Put(UpdateFilesRequest model)
        {
            long size = 0;
            var fileNames = new List<string>(model.Files.Count());

            foreach (var name in model.Files)
            {
                if (!nameFormat.IsMatch(name))
                {
                    continue;
                }

                var (path, meta) = BuildFileName(TemporaryPath, name);

                var fileInfo = new FileInfo(path);
                if (!fileInfo.Exists)
                {
                    continue;
                }

                var metaInfo = new FileInfo(meta);
                if (!metaInfo.Exists)
                {
                    continue;
                }

                var (finalPath, finalMeta) = BuildFileName(FinalPath, name);

                fileInfo.MoveTo(finalPath);
                metaInfo.MoveTo(finalMeta);

                size += fileInfo.Length;
                fileNames.Add(name);
            }

            return Ok(new UpdateFilesResponse { TotalSize = size, Files = fileNames });
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string name, bool download = false)
        {
            if (!nameFormat.IsMatch(name))
            {
                return BadRequest();
            }

            var (path, meta) = BuildFileName(FinalPath, name);

            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return NotFound();
            }

            var metaInfo = new FileInfo(meta);
            if (!metaInfo.Exists)
            {
                return NotFound();
            }

            FileMetadata metadata;

            using (var stream = metaInfo.OpenText())
            using (var reader = new JsonTextReader(stream))
            {
                var serializer = new JsonSerializer();
                metadata = serializer.Deserialize<FileMetadata>(reader);
            }

            if (download)
            {
                return File(fileInfo.Open(FileMode.Open), metadata.ContentType, metadata.FileName);
            }
            else
            {
                return File(fileInfo.Open(FileMode.Open), metadata.ContentType);
            }
        }

        private static (string path, string meta) BuildFileName(string folder, string name)
        {
            var path = Path.Combine(folder, $"{name}.file");
            var meta = Path.Combine(folder, $"{name}.meta");
            return (path, meta);
        }
    }
}