using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class ProductAttributeTemplateService : IProductAttributeTemplateService
    {
        #region Fields
        private readonly IProductAttributeTemplateRepository _templateRepository;
        #endregion

        #region Contractors
        public ProductAttributeTemplateService(IProductAttributeTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddRangeAsync(List<ProductAttributeTemplate> templates)
        {
            await _templateRepository.AddRangeAsync(templates);
            return true;
        }

        public async Task<bool> DeleteAsync(ProductAttributeTemplate template)
        {
            var transaction = _templateRepository.BeginTransaction();
            try
            {
                await _templateRepository.DeleteAsync(template);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        public async Task<bool> DeleteRangeAsync(List<ProductAttributeTemplate> templates)
        {
            var transaction = _templateRepository.BeginTransaction();
            try
            {
                await _templateRepository.DeleteRangeAsync(templates);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<ProductAttributeTemplate?> GetByIdAsync(int id)
        {
            return await _templateRepository.GetByIdAsync(id);
        }

        public async Task<List<ProductAttributeTemplate>> GetByCategoryIdAsync(int categoryId)
        {
            return await _templateRepository.GetByCategoryIdAsync(categoryId);
        }

        public async Task<bool> ExistsAsync(int categoryId, string keyEn)
        {
            return await _templateRepository.ExistsAsync(categoryId, keyEn);
        }

        public async Task<bool> HasVariantAttributesAsync(int templateId)
        {
            return await _templateRepository.HasVariantAttributesAsync(templateId);
        }
        public async Task<bool> CategoryHasVariantsAsync(int categoryId)
        {
            return await _templateRepository.CategoryHasVariantsAsync(categoryId);
        }
        #endregion
    }
}
