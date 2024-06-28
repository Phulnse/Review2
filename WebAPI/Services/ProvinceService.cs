using Application.IServices;
using Application.ViewModels.ProvinceVMs;
using AutoMapper;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProvinceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<GetProvinceVM> GetAll()
        {
            return _mapper.Map<List<GetProvinceVM>>(_unitOfWork.Province.GetAll());
        }
    }
}
