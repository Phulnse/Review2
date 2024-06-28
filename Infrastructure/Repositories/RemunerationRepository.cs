using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class RemunerationRepository : GenericRepository<Remuneration>, IRemunerationRepository
    {
        public RemunerationRepository(SRMSContext context) : base(context)
        {
        }

        public async Task CreateRemunerationAsync(Remuneration remuneration)
        {
            await AddAsync(remuneration);
        }

        public Remuneration GetLastRemuneration(Guid topicId)
        {
            return Find(x => x.TopicId.Equals(topicId) && x.IsAccepted == null).First();
        }

        public void UpdateRemuneration(Remuneration remuneration)
        {
            Update(remuneration);
        }
    }
}
