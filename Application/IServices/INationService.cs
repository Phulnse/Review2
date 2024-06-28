using Application.ViewModels.NationVMs;

namespace Application.IServices
{
    public interface INationService
    {
        IEnumerable<GetNationVM> GetAll();
    }
}
