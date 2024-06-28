using Application.ViewModels.CategoryVMs;

namespace Application.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetAllCategoriesRes>> GetAllCategoriesAsync();
        Task<CreateCategoryRes> CreateCategoryAsync(CreateCategoryReq req);
        Task<bool> UpdateCategoryAsync(UpdateCategoryReq req);
        Task<bool> DeleteCategoryAsync(Guid categoryId);
    }
}
