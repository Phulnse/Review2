using Application.IServices;
using Application.Utils;
using Application.ViewModels.ContractVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContractService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task UploadContractForEndingPhaseAsync(UploadContractForEndingPhaseReq req)
        {
            var contract = _mapper.Map<Contract>(req);
            contract.Id = Guid.NewGuid();
            contract.State = ContractStateEnum.FinaltermContract;
            var topic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            topic.SetSateAndProgress(24);

            await _unitOfWork.Contract.AddContractAsync(contract);
            await _unitOfWork.Save();
        }

        public async Task UploadEarlyContractAsync(UploadEarlyContractReq req)
        {
            var contract = _mapper.Map<Contract>(req);
            contract.Id = Guid.NewGuid();
            contract.State = ContractStateEnum.EarlyTermContract;
            var topic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            topic.SetSateAndProgress(9);

            await _unitOfWork.Contract.AddContractAsync(contract);
            await _unitOfWork.Save();
        }
    }
}
