using Application.IServices;
using Application.ViewModels.NotifyVMs;
using AutoMapper;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class NotifyService : INotifyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotifyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public GetNotifyRes GetNotifiesOfTopicForOwner(GetNotifyReq req)
        {
            var result = new GetNotifyRes();
            var notifies = _unitOfWork.Topic.GetNotifiesOfTopicForOwner(req.UserId);
            result.Notifies = _mapper.Map<List<NotifyVM>>(notifies);
            result.UnreadNotificationsNumber = notifies.Where(x => !x.HasRead).Count();

            return result;
        }
    }
}
