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
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ProductCommandHandler(IProductService productService,
                                     IProduct_DiscountService product_DiscountService,
                                     IFileService fileService,
                                     ICurrentUserService currentUserService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productService = productService;
            _product_DiscountService = product_DiscountService;
            _fileService = fileService;
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

            product.HasVariants = false;
            product.MinPrice = product.BasePrice;
            product.MaxPrice = product.BasePrice;

            var productId = await _productService.AddAsync(product);

            return productId > 0
                ? Success(productId, _localizer[SharedResourcesKeys.Created])
                : BadRequest<int>();
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

            if (!existingProduct.HasVariants)
            {
                existingProduct.MinPrice = existingProduct.BasePrice;
                existingProduct.MaxPrice = existingProduct.BasePrice;
            }

            existingProduct.UpdatedBy = currentUser.UserName;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            var isSuccess = await _productService.EditAsync(existingProduct);

            return isSuccess
                ? Success<string>(_localizer[SharedResourcesKeys.Updated])
                : BadRequest<string>();
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

            return isDeleted
                ? Success<string>(_localizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }
        #endregion
    }
}
