namespace BusinessServiceLayer.DTOs
{
    public class SystemAccountDTO
    {
        public int? Id { get; set; }
        public string? AccountName { get; set; }

        public string? AccountEmail { get; set; }

        public string? AccountPassword { get; set; }

        public string? ImageUrl { get; set; }

        public string? Gender { get; set; }

        public string? AccountRole { get; set; }
    }
}
