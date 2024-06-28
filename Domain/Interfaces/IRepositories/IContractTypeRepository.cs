using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.IRepositories
{
    public interface IContractTypeRepository
    {
        Task<List<ContractType>> GetContractTypeByStateAsync(ContractStateEnum contractState);
    }
}
