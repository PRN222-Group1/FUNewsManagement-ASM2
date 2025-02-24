namespace DataAccessLayer.Specifications.Account
{
    public class AccountSpecParams : PagingParams
    {
        public string Sort { get; set; }

        public int? Role { get; set; }

        private string _search;

        public string Search
        {
            get => _search;
            set => _search = (value != null) ? value.ToLower() : "";
        }
    }
}
