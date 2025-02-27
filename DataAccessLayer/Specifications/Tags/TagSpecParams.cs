namespace DataAccessLayer.Specifications.Tags
{
    public class TagSpecParams : PagingParams
    {
        public string? Sort { get; set; }

        private string _search;

        public string? Search
        {
            get => _search;
            set => _search = (value != null) ? value.ToLower() : "";
        }
    }
}
