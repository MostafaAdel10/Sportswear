using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IProductAttributeTemplateService
    {
        public Task<bool> AddRangeAsync(List<ProductAttributeTemplate> templates);
        public Task<bool> DeleteAsync(ProductAttributeTemplate template);
        public Task<bool> DeleteRangeAsync(List<ProductAttributeTemplate> templates);
        public Task<ProductAttributeTemplate?> GetByIdAsync(int id);
        public Task<List<ProductAttributeTemplate>> GetByCategoryIdAsync(int categoryId);
        public Task<bool> ExistsAsync(int categoryId, string keyEn);
        public Task<bool> HasVariantAttributesAsync(int templateId);
        public Task<bool> CategoryHasVariantsAsync(int categoryId);

    }
}
