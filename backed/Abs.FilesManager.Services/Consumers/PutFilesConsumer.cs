using Abs.Messages.FilesManager.Commands;
using Abs.Messages.FilesManager.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Abs.FilesManager.Services.Consumers
{
    public class PutFilesConsumer : IConsumer<IPutTemporaryFile>
    {
        private static readonly Regex fileNameFormat = new Regex("^[0-9]{14}-[0-9a-fA-F]{32}$");
        private readonly ILogger<PutFilesConsumer> logger;
        private readonly FilesConfig options;

        public PutFilesConsumer(ILogger<PutFilesConsumer> logger, IOptionsMonitor<FilesConfig> options)
        {
            this.logger = logger;
            this.options = options.CurrentValue;
        }

        public Task Consume(ConsumeContext<IPutTemporaryFile> context)
        {
            var code = context.Message.Code;

            if (!fileNameFormat.IsMatch(code))
            {
                throw new FormatException($"'{code}' has not file name format");
            }

            var fileName = Path.Combine(options.TemporaryFolder, code + ".file");
            var finalFileName = Path.Combine(options.FinalFolder, code + ".file");
            var metaFile = Path.Combine(options.TemporaryFolder, code + ".meta");
            var finalMetaFile = Path.Combine(options.FinalFolder, code + ".meta");

            logger.LogDebug("Moving file from {source} to {target}", fileName, finalFileName);
            logger.LogDebug("Moving meta from {source} to {target}", metaFile, finalMetaFile);

            var file = new FileInfo(fileName);
            var meta = new FileInfo(metaFile);

            if (!file.Exists || !meta.Exists)
            {
                logger.LogWarning("meta or file do not exists {meta}|{file}", meta.FullName, file.FullName);
                return Task.CompletedTask;
            }

            file.MoveTo(finalFileName);
            meta.MoveTo(finalMetaFile);
            logger.LogDebug("File and meta moved");
            return context.Publish<IFileCreated>(new { Code = code });
        }
    }
}
