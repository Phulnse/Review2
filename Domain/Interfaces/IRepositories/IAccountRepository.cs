using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByEmailAsync(string email);
        Task CreateBulkAccountAsync(IEnumerable<Account> accounts);
        void UpdateBulkAccount(IEnumerable<Account> accounts);
        IEnumerable<string> GetInactiveEmails();
        IEnumerable<Account> GetAccountByEmails(IEnumerable<string> emails);
        bool IsExistedEmail(string email);
    }
}
