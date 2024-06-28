using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class NationRepository : INationRepository
    {
        private readonly SRMSContext _context;
        public NationRepository(SRMSContext context)
        {
            _context = context;
        }
        public IEnumerable<Nation> GetAll()
        {
            return _context.Nations.AsEnumerable();
        }

        public bool IsValidNationName(string nationName)
        {
            return _context.Nations.Where(x => x.NationName.Equals(nationName)).Any();
        }
    }
}
