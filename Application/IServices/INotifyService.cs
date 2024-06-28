using Application.ViewModels.NotifyVMs;

namespace Application.IServices
{
    public interface INotifyService
    {
        GetNotifyRes GetNotifiesOfTopicForOwner(GetNotifyReq req);
    }
}
