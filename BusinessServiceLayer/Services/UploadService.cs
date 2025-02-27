using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusinessServiceLayer.Services
{
    public class UploadService : IUploadService
    {
        private readonly IFileStorageService _fileStorageService;

        public UploadService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<UploadResultDTO> UploadFileAsync(
            IFormFile file,
            string directory,
            string filePrefix = "")
        {
            var result = new UploadResultDTO();

            if (file == null || file.Length == 0)
            {
                result.Success = false;
                result.ErrorMessage = "No file selected or file is empty";
                return result;
            }

            // Set default allowed extensions to images if none provided
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            // Validate file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                result.Success = false;
                result.ErrorMessage = $"File type not allowed. Allowed types: {string.Join(", ", allowedExtensions)}";
                return result;
            }

            try
            {
                // Generate a unique filename
                string uniqueFileName = string.IsNullOrEmpty(filePrefix)
                    ? $"{Guid.NewGuid()}{extension}"
                    : $"{filePrefix}_{Guid.NewGuid()}{extension}";

                // Save the file using the storage service
                using (var stream = file.OpenReadStream())
                {
                    result.RelativeUrl = await _fileStorageService.SaveFileAsync(stream, directory, uniqueFileName);
                }

                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Error uploading file: {ex.Message}";
                return result;
            }
        }

        public async Task<bool> DeleteFileAsync(string relativeUrl)
        {
            return await _fileStorageService.DeleteFileAsync(relativeUrl);
        }
    }
}