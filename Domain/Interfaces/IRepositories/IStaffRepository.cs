namespace Domain.Interfaces.IRepositories
{
    public interface IStaffRepository
    {
        bool CheckStaffExisted(Guid id);
    }
}
