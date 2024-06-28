using Application.ViewModels.ProvinceVMs;

namespace Application.IServices
{
    public interface IProvinceService
    {
        List<GetProvinceVM> GetAll();
    }
}
