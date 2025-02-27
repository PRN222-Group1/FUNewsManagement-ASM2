using BusinessServiceLayer.Interfaces;

namespace Group1RazorPages.Services
{
    public class WebFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;

        public WebFileStorageService(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _webHostEnvironment = webHostEnvironment;
            _config = config;
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string directory, string fileName)
        {
            var uploadPath = _config["UploadUrl"];

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath, directory);

            // Clean the directory first (delete all existing files)
            if (Directory.Exists(uploadsFolder))
            {
                foreach (var file in Directory.GetFiles(uploadsFolder))
                {
                    File.Delete(file);
                }
            }
            else
            {
                // Make sure the directory exists
                Directory.CreateDirectory(uploadsFolder);
            }

            // Save the new file
            string filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            return $"/{uploadPath}/{directory}/{fileName}";
        }

        public Task<bool> DeleteFileAsync(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath))
                    return Task.FromResult(false);

                // Remove leading slash if present
                if (relativePath.StartsWith("/"))
                    relativePath = relativePath.Substring(1);

                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                if (!File.Exists(filePath))
                    return Task.FromResult(false);

                File.Delete(filePath);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

        }
    }
}
