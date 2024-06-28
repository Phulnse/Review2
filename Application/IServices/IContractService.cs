using Application.ViewModels.ContractVMs;

namespace Application.IServices
{
    public interface IContractService
    {
        Task UploadEarlyContractAsync(UploadEarlyContractReq req);
        Task UploadContractForEndingPhaseAsync(UploadContractForEndingPhaseReq req);
    }
}
