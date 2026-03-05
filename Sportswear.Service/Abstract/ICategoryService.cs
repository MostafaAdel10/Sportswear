using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface ICategoryService
    {
        public Task<bool> IsCategoryIdExist(int categoryId);
        public Task<List<Category>> GetCategoriesListAsync();
        public Task<Category> GetByIdAsync(int id);
        public Task<int> AddAsync(Category category);
        public Task<bool> EditAsync(Category category);
        public Task<bool> DeleteAsync(Category category);
    }
}
