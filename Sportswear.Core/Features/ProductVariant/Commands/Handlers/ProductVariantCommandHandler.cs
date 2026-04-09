using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ProductVariant.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.ProductVariant.Commands.Handlers
{
    public class ProductVariantCommandHandler : ResponseHandler,
        IRequestHandler<CreateProductVariantRangeCommand, Response<string>>,
        IRequestHandler<EditProductVariantCommand, Response<string>>,
        IRequestHandler<DeleteProductVariantCommand, Response<string>>
    {
        #region Fields
        private readonly IProductVariantService _productVariantService;
        private readonly IProductService _productService;
        private readonly ISkuGeneratorService _skuGeneratorService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ProductVariantCommandHandler(
            IProductVariantService productVariantService,
            IProductService productService,
            ISkuGeneratorService skuGeneratorService,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _productVariantService = productVariantService;
            _productService = productService;
            _skuGeneratorService = skuGeneratorService;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(CreateProductVariantRangeCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            var existingKeys = await _productVariantService.GetVariantKeysAsync(request.ProductId);
            var requestKeys = new HashSet<string>();
            var variants = new List<DataAccess.Entities.ProductVariant>();

            foreach (var dto in request.Variants)
            {
                // ✅ الـ Unique Key بيتعمل من AttributeValueEn + ColorHex
                var key = $"{dto.AttributeValueEn?.ToUpper()}-{dto.ColorHex?.ToUpper()}";

                if (!requestKeys.Add(key))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.ProductVariantDuplicateInRequest]);

                if (existingKeys.Contains(key))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.ProductVariantAlreadyExists]);

                // ✅ Generate SKU
                var skuParts = new List<string>();
                if (!string.IsNullOrEmpty(dto.AttributeValueEn))
                    skuParts.Add(dto.AttributeValueEn);
                if (!string.IsNullOrEmpty(dto.ColorLabel))
                    skuParts.Add(dto.ColorLabel);

                var sku = _skuGeneratorService.Generate(product.Code, skuParts);

                variants.Add(new DataAccess.Entities.ProductVariant
                {
                    ProductId = request.ProductId,
                    SKU = sku,
                    AttributeValueEn = dto.AttributeValueEn,
                    AttributeValueAr = dto.AttributeValueAr,
                    Unit = dto.Unit,
                    ColorLabel = dto.ColorLabel,
                    ColorHex = dto.ColorHex,
                    Price = dto.Price > 0 ? dto.Price : product.BasePrice,
                    StockQuantity = dto.StockQuantity,
                    CreatedBy = currentUser.UserName
                });
            }

            var isAdded = await _productVariantService.AddRangeAsync(variants);
            return isAdded ? Created("") : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditProductVariantCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var variant = await _productVariantService.GetByIdAsync(request.Id);
            if (variant == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            // ✅ Duplicate Check
            var newKey = $"{request.AttributeValueEn?.ToUpper()}-{request.ColorHex?.ToUpper()}";
            var existingKeys = await _productVariantService.GetVariantKeysAsync(variant.ProductId, excludeId: request.Id);
            if (existingKeys.Contains(newKey))
                return BadRequest<string>(_localizer[SharedResourcesKeys.ProductVariantAlreadyExists]);

            // ✅ Regenerate SKU
            var skuParts = new List<string>();
            if (!string.IsNullOrEmpty(request.AttributeValueEn))
                skuParts.Add(request.AttributeValueEn);
            if (!string.IsNullOrEmpty(request.ColorLabel))
                skuParts.Add(request.ColorLabel);

            var product = await _productService.GetByIdAsync(variant.ProductId);
            variant.SKU = _skuGeneratorService.Generate(product.Code, skuParts);
            variant.AttributeValueEn = request.AttributeValueEn;
            variant.AttributeValueAr = request.AttributeValueAr;
            variant.Unit = request.Unit;
            variant.ColorLabel = request.ColorLabel;
            variant.ColorHex = request.ColorHex;
            variant.Price = request.Price;
            variant.StockQuantity = request.StockQuantity;
            variant.UpdatedBy = currentUser.UserName;
            variant.UpdatedAt = DateTime.UtcNow;

            var isUpdated = await _productVariantService.EditAsync(variant);
            return isUpdated
                ? Success<string>(_localizer[SharedResourcesKeys.Updated])
                : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var variant = await _productVariantService.GetByIdWithIncludesAsync(request.Id);
            if (variant == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.VariantNotFound]);

            // مش هينفع تحذف variant عنده orders
            if (variant.OrderItems != null && variant.OrderItems.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.CannotDeleteVariantWithOrders]);

            variant.IsDeleted = true;
            variant.UpdatedBy = currentUser.UserName;
            variant.UpdatedAt = DateTime.UtcNow;

            var isDeleted = await _productVariantService.EditAsync(variant);

            return isDeleted
                ? Success<string>(_localizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }
        #endregion
    }
}
