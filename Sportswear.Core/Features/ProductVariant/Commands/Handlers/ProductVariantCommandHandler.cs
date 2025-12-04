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
                        IRequestHandler<CreateProductVariantCommand, Response<string>>,
                        IRequestHandler<EditProductVariantCommand, Response<string>>,
                        IRequestHandler<DeleteProductVariantCommand, Response<string>>
    {
        #region Fields
        private readonly IProductVariantService _productVariantService;
        private readonly IProductService _productService;
        private readonly IProduct_DiscountService _product_DiscountService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ProductVariantCommandHandler(IProductVariantService productVariantService, IProductService productService,
                                     IProduct_DiscountService product_DiscountService,
                                     ICurrentUserService currentUserService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productVariantService = productVariantService;
            _productService = productService;
            _product_DiscountService = product_DiscountService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            var productVariantMapping = _mapper.Map<DataAccess.Entities.ProductVariant>(request);

            productVariantMapping.CreatedBy = currentUser.UserName;

            // Price logic
            productVariantMapping.Price = productVariantMapping.Price > 0 ? productVariantMapping.Price : product.BasePrice;

            var isAdded = await _productVariantService.AddAsync(productVariantMapping);

            return isAdded ? Created("") : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditProductVariantCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var variant = await _productVariantService.GetByIdWithIncludesAsync(request.Id);
            if (variant == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.VariantNotFound]);

            if (variant.OrderItems.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.CannotEditVariantWithOrders]);

            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            _mapper.Map(request, variant);
            variant.UpdatedBy = currentUser.UserName;
            variant.UpdatedAt = DateTime.UtcNow;

            variant.Price = variant.Price > 0 ? variant.Price : product.BasePrice;

            var isEdited = await _productVariantService.EditAsync(variant);
            return isEdited ? Success<string>(_localizer[SharedResourcesKeys.Updated]) : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var variant = await _productVariantService.GetByIdWithIncludesAsync(request.Id);
            if (variant == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.VariantNotFound]);

            if (variant.OrderItems.Any())
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
