using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface INationRepository
    {
        IEnumerable<Nation> GetAll();
        bool IsValidNationName(string nationName);
    }
}
