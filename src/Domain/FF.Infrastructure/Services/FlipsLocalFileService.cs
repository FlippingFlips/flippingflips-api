using FF.Core;
using FF.Core.Interface;

namespace FF.Api.Services
{
    public class FlipsLocalFileService : IFileService
    {
        private string MEDIAPATH  = "media";

        /// <summary>
        /// Deletes a file from the webroot/media <see cref="MediaPath"/>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DeleteFileAsync(string fileName)
        {
            var file = Path.Combine(GlobalFlipsConfig.WebRootPath, MEDIAPATH, fileName);
            if (File.Exists(file)) { await Task.Run(() => File.Delete(file)); }
        }

        public async Task<string> SaveFileAsync(Stream stream, string fileName, string? mime = null)
        {
            var file = GetFilePath(fileName);
            var dir = Path.GetDirectoryName(file);
            if (dir != null)
            {
                if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
                using (var fs = new FileStream(GetFilePath(file), FileMode.Create))
                await stream.CopyToAsync(fs);
                return file;
            }
            else { throw new NullReferenceException($"No directory found for file: {file}"); }

        }

        public string GetFileUrl(string fileName) => $"/{MEDIAPATH}/{fileName.Replace(@"\","/")}";

        string GetFilePath(string fileName) => Path.Combine(GlobalFlipsConfig.WebRootPath, MEDIAPATH, fileName);
    }
}
