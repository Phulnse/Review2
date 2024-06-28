using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface INotifyRepository
    {
        IEnumerable<Notify> GetNonReadNotifies();
        void MarkToSendEmail(Guid notifyId);
    }
}
