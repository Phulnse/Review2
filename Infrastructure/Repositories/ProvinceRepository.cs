using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly SRMSContext _context;
        public ProvinceRepository(SRMSContext context)
        {
            _context = context;
        }
        public IEnumerable<Province> GetAll()
        {
            return _context.Provinces.AsEnumerable();
        }

        public bool IsValidProvinceName(string provinceName)
        {
            return _context.Provinces.Where(x => x.ProvinceName.Equals(provinceName)).Any();
        }
    }
}
