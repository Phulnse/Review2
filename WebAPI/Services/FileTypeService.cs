using Application.IServices;
using Application.ViewModels.FileTypeVMs;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class FileTypeService : IFileTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<GetFileTypeRes>> GetFileTypeListAsync(int stateNumber)
        {
            var state = (FileStateEnum)stateNumber;
            return _mapper.Map<List<GetFileTypeRes>>(await _unitOfWork.FileType.GetFileTypeAsync(state));
        }
    }
}
