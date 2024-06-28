using Application.IServices;
using Application.ViewModels.NationVMs;
using AutoMapper;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class NationService : INationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<GetNationVM> GetAll()
        {
            return _mapper.Map<List<GetNationVM>>(_unitOfWork.Nation.GetAll());
        }
    }
}
