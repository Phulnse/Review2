using Domain.Interfaces;
using Quartz;

namespace WebAPI.Services.JobServices
{
    public class MakeDefaultDecisionOverdueJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MakeDefaultDecisionOverdueJob> _logger;
        public MakeDefaultDecisionOverdueJob(IUnitOfWork unitOfWork, ILogger<MakeDefaultDecisionOverdueJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("MakeDefaultDecisionOverdueJob execute");
            var dateTimeNow = DateTime.Now;
            await _unitOfWork.MemberReview.SetDefaultValueForMemberNotResponse(dateTimeNow);
            await _unitOfWork.Save();
            await Task.CompletedTask;
        }
    }
}
