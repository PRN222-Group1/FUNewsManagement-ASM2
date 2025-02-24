using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.Account
{
    public class AccountCountSpecification : BaseSpecification<SystemAccount>
    {
        public AccountCountSpecification(AccountSpecParams specParams) : base(x =>
                (string.IsNullOrEmpty(specParams.Search)
                || x.AccountName.Contains(specParams.Search)
                || x.AccountEmail.Contains(specParams.Search))
                && (!specParams.Role.HasValue
                || x.AccountRole == specParams.Role)
            )
        {
        }
    }
}
