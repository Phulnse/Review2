using Application.ViewModels.DepartmentVMs;

namespace Application.IServices
{
    public interface IDepartmentService
    {
        Task<List<GetDepartmentReq>> GetDepartmentsAsync();
        Task CreateDepartmentAsync(CreateDepartmentReq req);
        // update
        Task<bool> UpdateDepartmentAsync(UpdateDepartmentReq req);
        // delete
        Task<bool> DeleteDepartmentAsync(Guid departmentId);
    }
}
