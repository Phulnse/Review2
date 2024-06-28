using Application.ViewModels.UserVMs;

namespace Application.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserInforRes>> GetAllAsync();
        Task<List<UserInforRes>> GetUsersByRole(bool isDean, Guid userId);
        Task<List<UserInforRes>> GetUsersNotParticipatingTopicAsync(Guid topicId);
        Task<bool> IsExistedUserAsync(string email);
        Task AssignDeanAsync(AssignDeanReq req);
        Task CreateUserDataAsync(List<UserVM> userVMs);

        //CRUD
        Task<UserInforRes> GetByIdAsync(Guid userId);
        Task<bool> UpdateUserAsync(UpdateUserReq req);
        Task<bool> DeleteUserAsync(Guid userId);

    }
}
