using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.IRepositories
{
    public interface ITopicRepository
    {
        Task CreateTopicAsync(Topic topic);
        Task<Topic> GetTopicAsync(Guid topicId);
        Task<List<Topic>> GetTopicByStateAndProgressForStaffAsync(TopicStateEnum state, TopicProgressEnum progress);
        void UpdateTopic(Topic topic);
        void SummarizeTheResultsAsync(List<Guid> topics);
        int CountNumberOfFormatCode(string codeFormat);
        Task<List<Topic>> GetTopicDecidedByDeanIdAsync(Guid deanId);
        Task<Topic> GetTopicDetailAsync(Guid topicId);
        Task<Topic> GetTopicDocumentAsync(Guid topicId);
        Task<Document> GetCurrentDocumentOfTopic(Guid topicId);
        Task<Topic> GetTopicProcessAsync(Guid topicId);
        Task<List<Topic>> GetTopicByStateAndProgressIncludeForStaffAsync(TopicStateEnum state, TopicProgressEnum progress);
        Task<List<Topic>> GetAllActiveTopicAsync();
        Task<bool> CheckTopicOwner(Guid userId, Guid topicId);
        Task<Topic?> GetMeetingReviewForLeader(Guid userId, Guid topicId);
        Task<bool> IsValidTopicAsync(Guid topicId, TopicStateEnum stateEnum, TopicProgressEnum progressEnum);
        Task<List<Topic>> GetTopicWaitingForUploadMeetingMinutesAsync();
        Task<List<Guid>> GetMemberIdOfTopicAsync(Guid topicId);
        Task<Guid> GetDepartmentIdOfTopicCreatorAsync(Guid topicId);
        Task<bool> IsValidToAddCouncilAsync(Guid topicId, List<Guid> councilIdList);
        Task<List<Topic>> GetTopicListAsync();
        IEnumerable<Notify> GetNotifiesOfTopicForOwner(Guid userId);
    }
}
