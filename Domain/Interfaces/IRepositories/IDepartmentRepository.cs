using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetDepartmentsAsync();
        Task CreateDepartmentAsync(Department department);
        bool IsvalidDepartmentName(string departmentName);
        bool IsExistDepartmentId(Guid departmentId);
        // update
        Task<Department> GetDepartmentByIdAsync(Guid departmentId);
        // delete
        Task DeleteDepartmentAsync(Guid departmentId);
    }
}
