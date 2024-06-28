using Application.IServices;
using Application.Utils;
using Application.ViewModels.RemunerationVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class RemunerationService : IRemunerationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RemunerationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CensorshipRemuneration(CensorshipRemunerationReq req)
        {
            var remuneration = _unitOfWork.Remuneration.GetLastRemuneration(req.TopicId);
            remuneration.IsAccepted = req.IsAccept;

            var topic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            topic.SetSateAndProgress(23);

            if (!req.IsAccept)
                topic.SetSateAndProgress(21);

            await _unitOfWork.Save();
        }

        public GetRemunerationRes GetRemunerationOfTopic(GetRemunerationReq req)
        {
            var remuneration = _unitOfWork.Remuneration.GetLastRemuneration(req.TopicId);
            return _mapper.Map<GetRemunerationRes>(remuneration);
        }

        public async Task SubmitRemunerationAsync(SubmitRemunerationReq req)
        {
            var remuneration = _mapper.Map<Remuneration>(req);
            remuneration.Id = Guid.NewGuid();
            await _unitOfWork.Remuneration.CreateRemunerationAsync(remuneration);
            var topic = await _unitOfWork.Topic.GetTopicAsync(req.TopicId);
            topic.SetSateAndProgress(22);

            await _unitOfWork.Save();
        }
    }
}
