namespace BusinessServiceLayer.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Saves a file to storage and returns the relative path
        /// </summary>
        Task<string> SaveFileAsync(Stream fileStream, string directory, string fileName);

        /// <summary>
        /// Deletes a file from storage
        /// </summary>
        Task<bool> DeleteFileAsync(string relativePath);

        /// <summary>
        /// Ensures a directory exists
        /// </summary>
        void EnsureDirectoryExists(string directory);
    }
}