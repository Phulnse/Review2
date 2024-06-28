using Domain.Interfaces;
using Quartz;

namespace WebAPI.Services.JobServices
{
    public class SumarizeResultJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SumarizeResultJob> _logger;

        public SumarizeResultJob(IUnitOfWork unitOfWork, ILogger<SumarizeResultJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("SumarizeResultJob execute");
            _unitOfWork.Topic.SummarizeTheResultsAsync(await _unitOfWork.MemberReview.GetTopicDecidedByAllMembersAsync());
            await _unitOfWork.Save();
            await Task.CompletedTask;
        }
    }
}
