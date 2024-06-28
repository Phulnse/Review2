using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IProvinceRepository
    {
        IEnumerable<Province> GetAll();
        bool IsValidProvinceName(string provinceName);
    }
}
