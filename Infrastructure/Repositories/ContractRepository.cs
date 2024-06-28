using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(SRMSContext context) : base(context)
        {
        }

        public async Task AddContractAsync(Contract contract)
        {
            await AddAsync(contract);
        }
    }
}
