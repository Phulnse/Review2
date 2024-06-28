using Application.ViewModels.ContractTypeVMs;

namespace Application.IServices
{
    public interface IContractTypeService
    {
        Task<List<GetContractTypeRes>> GetContractTypeByStateAsync(int stateNumber);
    }
}
