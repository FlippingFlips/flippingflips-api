namespace FF.Core.Interface
{
    public interface IFileService
    {        
        Task DeleteFileAsync(string fileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Web URL for file</returns>
        string GetFileUrl(string fileName);

        Task<string> SaveFileAsync(Stream stream, string fileName, string mime = null);
    }
}
