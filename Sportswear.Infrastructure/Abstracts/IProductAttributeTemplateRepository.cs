using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IProductAttributeTemplateRepository : IGenericRepositoryAsync<ProductAttributeTemplate>
    {
        public Task<List<ProductAttributeTemplate>> GetByCategoryIdAsync(int categoryId);
        public Task<bool> ExistsAsync(int categoryId, string keyEn);
        public Task<bool> HasVariantAttributesAsync(int templateId);
        public Task<bool> CategoryHasVariantsAsync(int categoryId);
    }
}
