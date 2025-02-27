using BusinessServiceLayer.DTOs;
using Microsoft.AspNetCore.Http;

namespace BusinessServiceLayer.Interfaces
{ 
    public interface IUploadService
    {
        /// <summary>
        /// Uploads a file to the specified directory and returns the relative URL
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="directory">The subdirectory under uploads (e.g., "profiles", "articles")</param>
        /// <param name="filePrefix">Optional prefix for the filename</param>
        /// <param name="allowedExtensions">Array of allowed file extensions</param>
        /// <returns>Result containing success status, relative URL if successful, and error message if failed</returns>
        Task<UploadResultDTO> UploadFileAsync(IFormFile file, string directory, string filePrefix = "");

        /// <summary>
        /// Deletes a file given its relative URL
        /// </summary>
        /// <param name="relativeUrl">The relative URL of the file to delete</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        Task<bool> DeleteFileAsync(string relativeUrl);
    }
}

