namespace BusinessServiceLayer.DTOs
{
    public class UploadResultDTO
    {
        public bool Success { get; set; }
        public string RelativeUrl { get; set; }
        public string ErrorMessage { get; set; }
    }
}
