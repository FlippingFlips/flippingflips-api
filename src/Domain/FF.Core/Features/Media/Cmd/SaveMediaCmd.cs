using FF.Core.Interface;
using MediatR;

namespace FF.Core.Features.Media.Cmd
{
    /// <summary>
    /// Saves a file to disk and returns the path it was saved to
    /// </summary>
    public class SaveMediaCmd : IRequest<string>
    {
        public SaveMediaCmd(string file, string dir, Stream fileStream = null, byte[] bytes = null)
        {
            FileStream = fileStream;
            Bytes = bytes;
            File = file;
            Dir = dir;
        }

        public string Dir { get; }
        public string File { get; }
        public Stream FileStream { get; }
        public byte[] Bytes { get; }        
    }

    internal class SaveMediaCmdHandler : IRequestHandler<SaveMediaCmd, string>
    {
        private readonly IFileService fileService;

        public SaveMediaCmdHandler(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public async Task<string> Handle(SaveMediaCmd request, CancellationToken cancellationToken)
        {
            if(request.Bytes?.Length > 0)
            {
                var blobName = Path.Combine(request.Dir, request.File);
                using (var stream = new MemoryStream(request.Bytes))
                return await fileService.SaveFileAsync(stream, blobName);
            }
            else if (request.FileStream?.Length > 0)
            {
                var blobName = Path.Combine(request.Dir, request.File);
                return await fileService.SaveFileAsync(request.FileStream, blobName);
            }

            throw new NullReferenceException("No stream or bytes found to save");

            //TODO Upload translite image azure
            /*
            if (game.TransliteData?.Length > 0)
            {
                logger.LogInformation("uploading translite blob...");
                var blobName = $"games/translite.jpg";
                using (var stream = new MemoryStream(game.TransliteData))
                {
                    stream.Position = 0;
                    var conn = configuration.GetConnectionString("AzureBlobConnection");
                    logger.LogInformation(conn);
                    var blobC = new BlobContainerClient(conn, configuration["data"]);
                    logger.LogInformation("container created");
                    //await blobC.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                    //logger.LogInformation("blob container good");
                    var client = blobC.GetBlobClient(blobName);
                    dbGame.Translite = client.Uri.AbsoluteUri;
                    logger.LogInformation("translite uploaded : " + blobName);
                }
            }
            */
        }
    }
}
