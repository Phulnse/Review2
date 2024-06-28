using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly SRMSContext _context;

        public UserRepository(SRMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Topic>> GetTopicForDeanAsync(Guid deanId)
        {
            return await Find(x => x.Id == deanId)
                         .Join(GetAll(),
                               dean => dean.DepartmentId,
                               users => users.DepartmentId,
                               (dean, users) => new { Result = users })
                         .Select(x => x.Result)
                         .SelectMany(x => x.Topics.Where(x => x.DeanDecision == null))
                         .Include(x => x.Category)
                         .ToListAsync();
        }

        public async Task<List<Topic>> GetTopicForUserReview(Guid userId, TopicStateEnum stateEnum, TopicProgressEnum progressEnum)
        {
            return await Find(x => x.Id.Equals(userId))
                         .SelectMany(x => x.Topics.Where(x => x.State == stateEnum && x.Progress == progressEnum))
                         .Include(x => x.Category)
                         .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await GetAll().Include(x => x.Department).ToListAsync();
        }

        public async Task<List<User>> GetUsersByRole(bool isDean)
        {
            return await Find(x => x.IsDean == isDean).Include(x => x.Department).ToListAsync();
        }

        public async Task<List<Topic>> GetTopicForUserAsync(Guid userId)
        {
            var joinedTTopic = Find(x => x.Id.Equals(userId))
                                .Include(x => x.Participants)
                                .ThenInclude(x => x.Topic)
                                .ThenInclude(x => x.Category)
                                .AsSplitQuery()
                                .AsNoTracking()
                                .SelectMany(x => x.Participants)
                                .Select(x => x.Topic);
            var myTopic = Find(x => x.Id.Equals(userId))
                            .Include(x => x.Topics)
                            .ThenInclude(x => x.Category)
                            .AsSplitQuery()
                            .AsNoTracking()
                            .SelectMany(x => x.Topics);

            return await joinedTTopic.Union(myTopic).ToListAsync();
        }

        public async Task<bool> IsExistedUserAsync(Guid userId)
        {
            return await Find(x => x.Id.Equals(userId)).AnyAsync();
        }

        public async Task<bool> IsDeanOfDepartmentAsync(Guid deanId, Guid departmentId)
        {
            return await Find(x => x.IsDean && x.Id.Equals(deanId) && x.DepartmentId.Equals(departmentId)).AnyAsync();
        }

        public async Task CreateBulkUserAsync(IEnumerable<User> users)
        {
            await AddBulkAsync(users);
        }

        public async Task<bool> IsExistedUserAsync(string email)
        {
            return await Find(x => x.AccountEmail.Equals(email)).AnyAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await Find(x => x.Id.Equals(userId)).FirstAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await Find(x => x.AccountEmail.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task<User?> GetDeanOfDepartmentAsync(Guid departmentId)
        {
            return await Find(x => x.DepartmentId.Equals(departmentId) && x.IsDean).FirstOrDefaultAsync();
        }

        public bool IsValidToAssignDean(string email)
        {
            return Find(x => x.AccountEmail.Equals(email) && !x.IsDean).FirstOrDefault() != null;
        }

        // New CRUD methods
        public async Task CreateUserAsync(User user)
        {
            await AddAsync(user);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }
    }
}
