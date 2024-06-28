using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(SRMSContext context) : base(context)
        {
        }

        public async Task CreateBulkAccountAsync(IEnumerable<Account> accounts)
        {
            await AddBulkAsync(accounts);
        }

        public IEnumerable<Account> GetAccountByEmails(IEnumerable<string> emails)
        {
            return Find(x => emails.Contains(x.Email)).AsEnumerable();
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await Find(x => x.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public IEnumerable<string> GetInactiveEmails()
        {
            return Find(x => !x.IsActive).Select(x => x.Email);
        }

        public void UpdateBulkAccount(IEnumerable<Account> accounts)
        {
            UpdateBulk(accounts);
        }

        public bool IsExistedEmail(string email)
        {
            return Find(x => x.Email.Equals(email)).Any();
        }
    }
}
