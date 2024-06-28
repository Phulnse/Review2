using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IRemunerationRepository
    {
        Task CreateRemunerationAsync(Remuneration remuneration);
        void UpdateRemuneration(Remuneration remuneration);
        Remuneration GetLastRemuneration(Guid topicId);
    }
}
