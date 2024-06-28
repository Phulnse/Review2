using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IContractRepository
    {
        Task AddContractAsync(Contract contract);
    }
}
