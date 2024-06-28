using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class NotifyRepository : GenericRepository<Notify>, INotifyRepository
    {
        public NotifyRepository(SRMSContext context) : base(context)
        {
        }

        public IEnumerable<Notify> GetNonReadNotifies()
        {
            return Find(x => !x.IsSendEmail)
                    .Include(x => x.Topic)
                    .ThenInclude(x => x.Creator)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .AsEnumerable();
        }

        public void MarkToSendEmail(Guid notifyId)
        {
            var notify = Find(x => x.Id == notifyId).First();
            notify.IsSendEmail = true;
        }
    }
}
