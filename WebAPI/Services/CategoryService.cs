using Application.IServices;
using Application.ViewModels.CategoryVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;


namespace WebAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllCategoriesRes>> GetAllCategoriesAsync()
        {
            var categoriesViewModel = _mapper.Map<IEnumerable<GetAllCategoriesRes>>(await _unitOfWork.Category.GetCategories());
            return categoriesViewModel;
        }

        public async Task<CreateCategoryRes> CreateCategoryAsync(CreateCategoryReq req)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = req.CategoryName
            };

            await _unitOfWork.Category.CreateCategoryAsync(category);
            await _unitOfWork.Save();

            return new CreateCategoryRes { Id = category.Id, CategoryName = category.CategoryName };
        }

        public async Task<bool> UpdateCategoryAsync(UpdateCategoryReq req)
        {
            var category = await _unitOfWork.Category.GetCategoryByIdAsync(req.CategoryId);
            if (category == null)
            {
                return false; // No ID
            }

            category.CategoryName = req.CategoryName;
            await _unitOfWork.Save();
            return true; // Success
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _unitOfWork.Category.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                return false; // No ID
            }

            await _unitOfWork.Category.DeleteCategoryAsync(categoryId);
            await _unitOfWork.Save();
            return true; // Success
        }
    }
}
