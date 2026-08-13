using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;
using Sportswear.Service.Implementations;

namespace Sportswear.Core.Features.Product.Commands.Handlers
{
    public class ProductCommandHandler : ResponseHandler,
                        IRequestHandler<CreateProductCommand, Response<int>>,
                        IRequestHandler<EditProductCommand, Response<string>>,
                        IRequestHandler<DeleteProductCommand, Response<string>>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IProduct_DiscountService _product_DiscountService;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICacheService _cacheService;
        #endregion

        #region Constructors
        public ProductCommandHandler(IProductService productService,
                                     IProduct_DiscountService product_DiscountService,
                                     IFileService fileService,
                                     ICurrentUserService currentUserService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> stringLocalizer,
                                     ICacheService cacheService) : base(stringLocalizer)
        {
            _productService = productService;
            _product_DiscountService = product_DiscountService;
            _fileService = fileService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _localizer = stringLocalizer;
            _cacheService = cacheService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<int>();

            var product = _mapper.Map<DataAccess.Entities.Product>(request);

            product.CreatedBy = currentUser.UserName;

            product.HasVariants = false;
            product.MinPrice = product.BasePrice;
            product.MaxPrice = product.BasePrice;

            var productId = await _productService.AddAsync(product);

            if (productId > 0)
            {
                InvalidateProductsListCache();
                return Success(productId, _localizer[SharedResourcesKeys.Created]);
            }
            return BadRequest<int>();
        }

        public async Task<Response<string>> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // Check if product exists
            var existingProduct = await _productService.GetByIdWithIncludesAsync(request.Id);
            if (existingProduct == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            // احفظ الـ Code الأصلي قبل الـ mapping لأن Edit ممكن يغيّره
            var oldCode = existingProduct.Code;

            // Map new values to existing entity
            existingProduct = _mapper.Map(request, existingProduct);

            if (!existingProduct.HasVariants)
            {
                existingProduct.MinPrice = existingProduct.BasePrice;
                existingProduct.MaxPrice = existingProduct.BasePrice;
            }

            existingProduct.UpdatedBy = currentUser.UserName;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            var isSuccess = await _productService.EditAsync(existingProduct);

            if (isSuccess)
            {
                // ✅ Clear all product-related cache (كل الـ culture variants)
                InvalidateProductCache(request.Id, oldCode);

                // لو الـ Code اتغير في الـ Edit، امسح المفتاح الجديد كمان
                if (!string.Equals(oldCode, existingProduct.Code, StringComparison.OrdinalIgnoreCase))
                    InvalidateProductByCodeCache(existingProduct.Code);

                return Success<string>(_localizer[SharedResourcesKeys.Updated]);
            }
            return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();

            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var existingProduct = await _productService.GetByIdWithIncludesAsync(request.Id);
            if (existingProduct == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            if (existingProduct.Variants.Any(v => !v.IsDeleted))
                return BadRequest<string>(_localizer[SharedResourcesKeys.CannotDeleteProductRelatedToVariants]);

            // Soft delete
            existingProduct.IsDeleted = true;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            existingProduct.UpdatedBy = currentUser.UserName;

            foreach (var variant in existingProduct.Variants)
                variant.IsDeleted = true;

            foreach (var img in existingProduct.Images)
            {
                _fileService.DeleteImage(img.Url);
                img.IsDeleted = true;
            }

            foreach (var review in existingProduct.Reviews)
                review.IsDeleted = true;

            // Hard delete product_discounts links
            // Hard delete لروابط الخصومات (batch delete بدون loop)
            var linksToDelete = existingProduct.Product_Discounts.ToList();
            if (linksToDelete.Any())
            {
                await _product_DiscountService.DeleteRangeAsync(linksToDelete); // افتراضيًا من GenericRepository
            }

            var isDeleted = await _productService.EditAsync(existingProduct);

            if (isDeleted)
            {
                // ✅ Clear all cache (كل الـ culture variants)
                InvalidateProductCache(request.Id, existingProduct.Code);
                return Success<string>(_localizer[SharedResourcesKeys.Deleted]);
            }
            return BadRequest<string>();
        }
        #endregion

        #region Cache Invalidation Helpers
        private static readonly string[] SupportedCultures = { "en", "ar" };

        private void InvalidateProductsListCache()
        {
            foreach (var culture in SupportedCultures)
                _cacheService.Remove($"{CacheKeys.ProductsList}_{culture}");
        }

        private void InvalidateProductByCodeCache(string code)
        {
            var baseKey = string.Format(CacheKeys.ProductByCode, code);
            foreach (var culture in SupportedCultures)
                _cacheService.Remove($"{baseKey}_{culture}");
        }

        /// <summary>
        /// بتمسح كل مفاتيح الكاش الخاصة بمنتج معين (List + ById + FullDetails + WithVariants + ByCode)
        /// عبر كل الـ cultures المدعومة.
        /// </summary>
        private void InvalidateProductCache(int id, string code)
        {
            InvalidateProductsListCache();

            var byIdKey = string.Format(CacheKeys.ProductById, id);
            var fullDetailsKey = string.Format(CacheKeys.ProductFullDetails, id);
            var withVariantsKey = string.Format(CacheKeys.ProductWithVariants, id);
            var byCodeKey = string.Format(CacheKeys.ProductByCode, code);

            foreach (var culture in SupportedCultures)
            {
                _cacheService.Remove($"{byIdKey}_{culture}");
                _cacheService.Remove($"{fullDetailsKey}_{culture}");
                _cacheService.Remove($"{withVariantsKey}_{culture}");
                _cacheService.Remove($"{byCodeKey}_{culture}");
            }
        }
        #endregion
    }
}