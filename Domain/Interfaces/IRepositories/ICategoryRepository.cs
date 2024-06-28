using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<bool> IsExistedCategoryAsync(Guid categoryId);
        Task CreateCategoryAsync(Category category);
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task DeleteCategoryAsync(Guid categoryId);
    }
}
