using Application.ViewModels.FileTypeVMs;

namespace Application.IServices
{
    public interface IFileTypeService
    {
        Task<List<GetFileTypeRes>> GetFileTypeListAsync(int stateNumber);
    }
}
