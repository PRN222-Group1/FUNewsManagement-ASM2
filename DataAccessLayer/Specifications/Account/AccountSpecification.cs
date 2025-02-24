using BusinessObjects.Entities;

namespace DataAccessLayer.Specifications.Account
{
    public class AccountSpecification : BaseSpecification<SystemAccount>
    {
        public AccountSpecification(string email) : base(x => x.AccountEmail == email)
        {
        }

        public AccountSpecification(LoginPayload loginPayload) :
            base(x => x.AccountEmail == loginPayload.AccountEmail 
                && x.AccountPassword == loginPayload.AccountPassword)
        {
        }

        public AccountSpecification(AccountSpecParams specParams) :
            base(x => 
                (string.IsNullOrEmpty(specParams.Search) 
                || x.AccountName.Contains(specParams.Search)
                || x.AccountEmail.Contains(specParams.Search))
                && (!specParams.Role.HasValue 
                || x.AccountRole == specParams.Role)
            )
        {
            ApplyPaging(specParams.PageSize * (specParams.PageNumber - 1),
                specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(sa => sa.AccountName);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(sa => sa.AccountName);
                        break;
                    default:
                        AddOrderBy(sa => sa.AccountName);
                        break;
                }
            }
        }
    }
}
