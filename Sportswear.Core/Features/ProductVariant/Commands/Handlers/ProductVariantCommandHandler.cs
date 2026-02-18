using AutoMapper;
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
        private readonly IProduct_DiscountService _product_DiscountService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ProductVariantCommandHandler(IProductVariantService productVariantService, IProductService productService,
                                     ISkuGeneratorService skuGeneratorService,
                                     IProduct_DiscountService product_DiscountService,
                                     ICurrentUserService currentUserService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productVariantService = productVariantService;
            _productService = productService;
            _skuGeneratorService = skuGeneratorService;
            _product_DiscountService = product_DiscountService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _localizer = stringLocalizer;
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

            var existingVariants = await _productVariantService.GetByProductIdAsync(request.ProductId);

            var existingSet = existingVariants
                .Select(x => $"{x.ColorName}-{x.Size}".ToUpper())
                .ToHashSet();

            var requestSet = new HashSet<string>();

            var variants = new List<DataAccess.Entities.ProductVariant>();

            foreach (var dto in request.Variants)
            {
                var key = $"{dto.ColorName}-{dto.Size}".ToUpper();

                if (!requestSet.Add(key))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.ProductVariantDuplicateInRequest]);

                if (existingSet.Contains(key))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.ProductVariantAlreadyExists]);

                var sku = _skuGeneratorService.Generate(product.Code, dto.ColorName, dto.Size);

                variants.Add(new DataAccess.Entities.ProductVariant
                {
                    ProductId = request.ProductId,
                    SKU = sku,
                    Size = dto.Size,
                    ColorName = dto.ColorName,
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

            var variant = await _productVariantService.GetByIdWithIncludesAsync(request.Id);
            if (variant == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            // check duplicate (except current variant)
            var exists = await _productVariantService.ExistsAsync(
                variant.ProductId,
                request.ColorName,
                request.Size,
                request.Id);

            if (exists)
                return BadRequest<string>(_localizer[SharedResourcesKeys.ProductVariantAlreadyExists]);

            // regenerate SKU
            variant.SKU = _skuGeneratorService.Generate(
                                variant.Product.Code,
                                request.ColorName,
                                request.Size);

            variant.Size = request.Size;
            variant.ColorName = request.ColorName;
            variant.ColorHex = request.ColorHex;
            variant.Price = request.Price;
            variant.StockQuantity = request.StockQuantity;

            variant.UpdatedBy = currentUser.UserName;
            variant.UpdatedAt = DateTime.UtcNow;

            var isUpdated = await _productVariantService.EditAsync(variant);

            return isUpdated
                ? Success<string>(_localizer[SharedResourcesKeys.Updated])
                : BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);
        }


        public async Task<Response<string>> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var variant = await _productVariantService.GetByIdWithIncludesAsync(request.Id);
            if (variant == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.VariantNotFound]);

            if (variant.OrderItems != null && variant.OrderItems.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.CannotDeleteVariantWithOrders]);

            variant.IsDeleted = true;
            variant.UpdatedBy = currentUser.UserName;
            variant.UpdatedAt = DateTime.UtcNow;

            var isDeleted = await _productVariantService.EditAsync(variant);
            return isDeleted ? Success<string>(_localizer[SharedResourcesKeys.Deleted]) : BadRequest<string>();
        }
        #endregion
    }
}
