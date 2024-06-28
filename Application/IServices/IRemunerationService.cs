using Application.ViewModels.RemunerationVMs;

namespace Application.IServices
{
    public interface IRemunerationService
    {
        Task SubmitRemunerationAsync(SubmitRemunerationReq req);
        Task CensorshipRemuneration(CensorshipRemunerationReq req);
        GetRemunerationRes GetRemunerationOfTopic(GetRemunerationReq req);
    }
}
