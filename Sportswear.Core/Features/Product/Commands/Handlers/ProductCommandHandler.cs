using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

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
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ProductCommandHandler(IProductService productService,
                                     IProduct_DiscountService product_DiscountService,
                                     ICurrentUserService currentUserService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productService = productService;
            _product_DiscountService = product_DiscountService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _localizer = stringLocalizer;
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

            var productId = await _productService.AddAsync(product);

            if (productId <= 0)
                return BadRequest<int>();
            else
                return Success(productId, _localizer[SharedResourcesKeys.Created]);
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

            // Map new values to existing entity
            existingProduct = _mapper.Map(request, existingProduct);

            existingProduct.UpdatedBy = currentUser.UserName;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            var isSuccess = await _productService.EditAsync(existingProduct);

            if (isSuccess)
                return Success<string>(_localizer[SharedResourcesKeys.Updated]);
            else
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

            if (existingProduct.Variants.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.CannotDeleteProductRelatedToVariants]);

            // Soft delete
            existingProduct.IsDeleted = true;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            existingProduct.UpdatedBy = currentUser.UserName;

            foreach (var variant in existingProduct.Variants)
                variant.IsDeleted = true;

            foreach (var img in existingProduct.Images)
                img.IsDeleted = true;

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

            return isDeleted
                ? Success<string>(_localizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }
        #endregion
    }
}
