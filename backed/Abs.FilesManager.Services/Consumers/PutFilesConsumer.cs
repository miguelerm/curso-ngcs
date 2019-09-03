using Abs.Messages.FilesManager.Commands;
using Abs.Messages.FilesManager.Events;
using MassTransit;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Abs.FilesManager.Services.Consumers
{
    public class PutFilesConsumer : IConsumer<IPutTemporaryFile>
    {
        private static readonly Regex fileNameFormat = new Regex("^[0-9]{14}-[0-9a-fA-F]{32}$");

        public Task Consume(ConsumeContext<IPutTemporaryFile> context)
        {
            var code = context.Message.Code;

            if (!fileNameFormat.IsMatch(code))
            {
                throw new Exception();
            }

            var folder = Path.GetTempPath();
            var finalFolder = Directory.GetCurrentDirectory();
            var fileName = Path.Combine(folder, code + ".file");
            var finalFileName = Path.Combine(finalFolder, "Files", code + ".file");
            var metaFile = Path.Combine(folder, code + ".meta");
            var finalMetaFile = Path.Combine(finalFolder, "Files", code + ".meta");

            var file = new FileInfo(fileName);
            var meta = new FileInfo(metaFile);

            if (!file.Exists || !meta.Exists)
            {
                return Task.CompletedTask;
            }

            file.MoveTo(finalFileName);
            meta.MoveTo(finalMetaFile);
            return context.Publish<IFileCreated>(new { Code = code });
        }
    }
}
