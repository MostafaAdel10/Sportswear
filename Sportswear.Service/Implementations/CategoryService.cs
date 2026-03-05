using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        #region Fields
        private readonly ICategoryRepository _categoryRepository;
        #endregion

        #region Contractors
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> IsCategoryIdExist(int categoryId)
        {
            return await _categoryRepository.IsCategoryIdExist(categoryId);
        }
        public async Task<List<Category>> GetCategoriesListAsync()
        {
            return await _categoryRepository.GetCategoriesListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category;
        }

        public async Task<int> AddAsync(Category category)
        {
            var savedCategory = await _categoryRepository.AddAsync(category);
            return savedCategory.Id;
        }

        public async Task<bool> EditAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
            return true;
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            var transaction = _categoryRepository.BeginTransaction();

            try
            {
                await _categoryRepository.DeleteAsync(category);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        #endregion
    }
}
