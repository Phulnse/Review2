using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class StaffRepository : GenericRepository<Staff>, IStaffRepository
    {
        public StaffRepository(SRMSContext context) : base(context)
        {
        }

        public bool CheckStaffExisted(Guid id)
        {
            return Find(x => x.Id.Equals(id)).Any();
        }
    }
}
