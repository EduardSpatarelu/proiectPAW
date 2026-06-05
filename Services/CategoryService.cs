using BARmonica.Web.Models;
using BARmonica.Web.Repositories;

namespace BARmonica.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        // Aici Bucatarul (Serviciul) cere ajutorul Magazionerului (Repository)
        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            // Aici s-ar pune reguli de business (ex: if category.Name == null arunca eroare)
            await _repository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _repository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}