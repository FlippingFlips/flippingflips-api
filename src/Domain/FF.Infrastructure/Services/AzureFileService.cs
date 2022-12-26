using FF.Core.Interface;

namespace FF.Infrastructure.Services
{
    public class AzureFileService : IFileService
    {
        public Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public string GetFileUrl(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveFileAsync(Stream stream, string fileName, string? mime = null)
        {
            throw new NotImplementedException();
        }
    }
}
