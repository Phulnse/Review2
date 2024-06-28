using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly SRMSContext _context;

        public CategoryRepository(SRMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<bool> IsExistedCategoryAsync(Guid categoryId)
        {
            return await Find(x => x.Id.Equals(categoryId)).AnyAsync();
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await AddAsync(category);
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            return await Find(x => x.Id.Equals(categoryId)).FirstOrDefaultAsync();
        }

        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            var category = await GetCategoryByIdAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
        }
    }
}
