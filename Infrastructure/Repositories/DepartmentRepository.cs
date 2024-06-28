using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly SRMSContext _context;

        public DepartmentRepository(SRMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateDepartmentAsync(Department department)
        {
            await AddAsync(department);
        }

        public Task<Department> GetDepartmentByIdAsync(Guid departmentId)
        {
            return Find(x => x.Id.Equals(departmentId)).FirstAsync();
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await GetAll().ToListAsync();
        }

        public bool IsExistDepartmentId(Guid departmentId)
        {
            return Find(x => x.Id.Equals(departmentId)).Any();
        }

        public bool IsvalidDepartmentName(string departmentName)
        {
            return Find(x => x.DepartmentName.Equals(departmentName)).Any();
        }

        public async Task DeleteDepartmentAsync(Guid departmentId)
        {
            var department = await GetDepartmentByIdAsync(departmentId);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
        }
    }
}
