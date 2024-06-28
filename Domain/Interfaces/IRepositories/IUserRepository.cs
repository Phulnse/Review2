using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<List<Topic>> GetTopicForDeanAsync(Guid deanId);
        Task<List<Topic>> GetTopicForUserReview(Guid userId, TopicStateEnum stateEnum, TopicProgressEnum progressEnum);
        Task<List<User>> GetUsersByRole(bool isDean);
        Task<List<Topic>> GetTopicForUserAsync(Guid userId);
        Task<bool> IsExistedUserAsync(Guid userId);
        Task<bool> IsDeanOfDepartmentAsync(Guid deanId, Guid departmentId);
        Task CreateBulkUserAsync(IEnumerable<User> users);
        Task<bool> IsExistedUserAsync(string email);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetDeanOfDepartmentAsync(Guid departmentId);
        bool IsValidToAssignDean(string email);

        // New CRUD methods
        Task CreateUserAsync(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
