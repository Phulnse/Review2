using Application.IServices;
using Application.ViewModels.ContractTypeVMs;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class ContractTypeService : IContractTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContractTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetContractTypeRes>> GetContractTypeByStateAsync(int stateNumber)
        {
            var state = (ContractStateEnum)stateNumber;

            var result = await _unitOfWork.ContractType.GetContractTypeByStateAsync(state);
            return _mapper.Map<List<GetContractTypeRes>>(result);
        }
    }
}
