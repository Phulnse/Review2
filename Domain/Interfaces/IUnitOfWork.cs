using Domain.Interfaces.IRepositories;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITopicRepository Topic { get; }
        IMemberReviewRepository MemberReview { get; }
        IParticipantRepository Participant { get; }
        ICategoryRepository Category { get; }
        IUserRepository User { get; }
        IReviewRepository Review { get; }
        ICouncilRepository Council { get; }
        IStaffRepository Staff { get; }
        IDocumentRepository Document { get; }
        IContractRepository Contract { get; }
        IAccountRepository Account { get; }
        IContractTypeRepository ContractType { get; }
        IFileTypeRepository FileType { get; }
        IDepartmentRepository Department { get; }
        IRemunerationRepository Remuneration { get; }
        IHolidayRepository Holiday { get; }
        IProvinceRepository Province { get; }
        INationRepository Nation { get; }
        INotifyRepository Notify { get; }
        Task BeginTransaction();
        Task Commit();
        Task Rollback();
        Task Save();
    }
}
