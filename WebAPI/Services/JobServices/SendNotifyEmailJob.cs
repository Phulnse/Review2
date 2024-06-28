using Application.IServices;
using Domain.Enums;
using Domain.Interfaces;
using Quartz;

namespace WebAPI.Services.JobServices
{
    public class SendNotifyEmailJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SendNotifyEmailJob> _logger;
        private readonly IEmailService _emailService;

        public SendNotifyEmailJob(IUnitOfWork unitOfWork, ILogger<SendNotifyEmailJob> logger, IEmailService emailService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("SendNotifyEmailJob execute");
            var notifies = _unitOfWork.Notify.GetNonReadNotifies();
            foreach (var item in notifies)
            {
                string message;
                string action = "";
                switch (item.State, item.Progress, item.IsReject)
                {
                    case (TopicStateEnum.PreliminaryReview, TopicProgressEnum.WaitingForCouncilFormation, true):
                        message = "Đã được trưởng phòng duyệt";
                        break;
                    case (TopicStateEnum.PreliminaryReview, TopicProgressEnum.WaitingForDean, false):
                        message = "Đề tài không được phê duyệt";
                        break;
                    case (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForConfigureConference, true):
                        message = "Đã được các thành viên thông qua";
                        break;
                    case (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForCouncilDecision, false):
                        message = "Không được các thành viên thông qua";
                        break;
                    case (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes, true):
                        message = "Đã tạo lịch meeting";
                        action = "Kiểm tra thời gian để tham gia meeting";
                        break;
                    case (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes, false):
                        message = "Đã có kết quả của hội đồng: \"Đề tài không được phê duyệt\"";
                        break;
                    case (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForDocumentEditing, true):
                        message = "Đã có kết quả của hội đồng: \"Đề tài cần chỉnh sửa\"";
                        action = "Chỉnh sửa tài liệu theo yêu cầu";
                        break;
                    case (TopicStateEnum.EarlyTermReport, TopicProgressEnum.WaitingForUploadContract, true):
                        message = "Đã có kết quả của hội đồng: \"Đề tài được thông qua\"";
                        break;
                    case (TopicStateEnum.EarlyTermReport, TopicProgressEnum.Completed, true):
                        message = "Hoàn thành giai đoạn đầu kì";
                        break;
                    case (TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForMakeReviewSchedule, true):
                        message = "Đề tài chuyển sang giai đoạn giữa kì";
                        break;
                    case (TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForDocumentSupplementation, true):
                        message = "Đã tạo lịch bổ sung tài liệu";
                        action = "Bổ sung tài liệu theo yêu cầu";
                        break;
                    case (TopicStateEnum.MidtermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes, true):
                        message = "Đã tạo lịch meeting";
                        action = "Kiểm tra thời gian để tham gia meeting";
                        break;
                    case (TopicStateEnum.MidtermReport, TopicProgressEnum.Completed, true):
                        message = "Hoàn thành giai đoạn giữa kì";
                        break;
                    case (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForDocumentSupplementation, true):
                        message = "Đã tạo lịch bổ sung tài liệu";
                        action = "Bổ sung tài liệu theo yêu cầu";
                        break;
                    case (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes, true):
                        message = "Đã tạo lịch meeting";
                        action = "Kiểm tra thời gian để tham gia meeting";
                        break;
                    case (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForUploadMeetingMinutes, false):
                        message = "Đã có kết quả của hội đồng: \"Đề tài không được phê duyệt\"";
                        break;
                    case (TopicStateEnum.FinaltermReport, TopicProgressEnum.WaitingForDocumentEditing, true):
                        message = "Đã có kết quả của hội đồng: \"Đề tài cần chỉnh sửa\"";
                        action = "Chỉnh sửa tài liệu theo yêu cầu";
                        break;
                    case (TopicStateEnum.EndingPhase, TopicProgressEnum.WaitingForSubmitRemuneration, true):
                        message = "Đã hoàn thành giai đoạn cuối kì";
                        action = "Nộp file tính thù lao";
                        break;
                    case (TopicStateEnum.EndingPhase, TopicProgressEnum.WaitingForUploadContract, true):
                        message = "File tính thù lao đã được duyệt";
                        break;
                    case (TopicStateEnum.EndingPhase, TopicProgressEnum.Completed, true):
                        message = "Đề tài đã hoàn tất";
                        break;
                    default:
                        message = "";
                        action = "";
                        break;
                }
                if (!string.IsNullOrEmpty(message))
                {
                    _emailService.SendNotifyEmailAsync(item.Topic.Creator.AccountEmail, "[Noreply] Thông báo trạng thái đề tài", message, action);
                    _unitOfWork.Notify.MarkToSendEmail(item.Id);
                }             
            }
            await _unitOfWork.Save();
            await Task.CompletedTask;
        }
    }
}
