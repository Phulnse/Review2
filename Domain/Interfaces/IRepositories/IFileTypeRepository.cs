using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.IRepositories
{
    public interface IFileTypeRepository
    {
        Task<List<FileType>> GetFileTypeAsync(FileStateEnum state);
    }
}
