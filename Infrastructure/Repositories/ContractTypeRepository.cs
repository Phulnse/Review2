using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContractTypeRepository : GenericRepository<ContractType>, IContractTypeRepository
    {
        public ContractTypeRepository(SRMSContext context) : base(context)
        {
        }

        public async Task<List<ContractType>> GetContractTypeByStateAsync(ContractStateEnum contractState)
        {
            return await Find(x => x.State == contractState).ToListAsync();
        }
    }
}
