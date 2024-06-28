using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FileTypeRepository : GenericRepository<FileType>, IFileTypeRepository
    {
        public FileTypeRepository(SRMSContext context) : base(context)
        {
        }

        public async Task<List<FileType>> GetFileTypeAsync(FileStateEnum stateEnum)
        {
            return await Find(x => x.State == stateEnum).ToListAsync();
        }
    }
}
