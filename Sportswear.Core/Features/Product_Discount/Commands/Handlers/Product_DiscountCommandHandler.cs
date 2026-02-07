using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product_Discount.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.DataAccess.Enums;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.Product_Discount.Commands.Handlers
{
    public class Product_DiscountCommandHandler : ResponseHandler,
                        IRequestHandler<AddDiscountToProductsCommand, Response<string>>,
                        IRequestHandler<RemoveDiscountFromProductsCommand, Response<string>>
    {
        #region Fields
        private readonly IProduct_DiscountService _product_DiscountService;
        private readonly IDiscountService _discountService;
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public Product_DiscountCommandHandler(IProductService productService,
                                     IDiscountService discountService,
                                     IProduct_DiscountService product_DiscountService,
                                     ICurrentUserService currentUserService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productService = productService;
            _discountService = discountService;
            _product_DiscountService = product_DiscountService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddDiscountToProductsCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // ✅ Check if Discount exists AND Active (single query)
            var discount = await _discountService.GetActiveDiscountByIdAsync(request.DiscountId);
            if (discount == null)
                return BadRequest<string>(_localizer[SharedResourcesKeys.DiscountIsNotActive]);

            if (discount.Type == DiscountType.Global)
                return BadRequest<string>(_localizer[SharedResourcesKeys.YouCanNotAddThisDiscountBecauseItIsGlobal]);

            // التحقق من المنتجات وجمعها
            var products = await _productService.GetByIdsAsync(request.ProductIds);
            if (products.Count != request.ProductIds.Count)
                return BadRequest<string>(_localizer[SharedResourcesKeys.SomeProductsNotFound]);

            // إنشاء الروابط الجديدة، مع تجنب التكرار
            var newLinks = new List<DataAccess.Entities.Product_Discount>();

            foreach (var product in products)
            {
                if (await _product_DiscountService.ExistsAsync(request.DiscountId, product.Id))
                    continue;

                newLinks.Add(new DataAccess.Entities.Product_Discount
                {
                    DiscountId = request.DiscountId,
                    ProductId = product.Id
                });
            }

            if (!newLinks.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.DiscountAlreadyApplied]);

            var isAdded = await _product_DiscountService.AddRangeAsync(newLinks);

            return isAdded ? Created("") : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(RemoveDiscountFromProductsCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // ✅ Discount must be active
            var discount = await _discountService.GetActiveDiscountByIdAsync(request.DiscountId);
            if (discount == null)
                return BadRequest<string>(_localizer[SharedResourcesKeys.DiscountIsNotActive]);

            // ✅ Validate products exist
            var products = await _productService.GetByIdsAsync(request.ProductIds);
            if (products.Count != request.ProductIds.Count)
                return BadRequest<string>(_localizer[SharedResourcesKeys.SomeProductsNotFound]);

            // ✅ Get links to delete
            var linksToDelete = await _product_DiscountService
                .GetByDiscountAndProductsAsync(request.DiscountId, request.ProductIds);

            if (!linksToDelete.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.NoLinksToDelete]);

            var isDeleted = await _product_DiscountService.DeleteRangeAsync(linksToDelete);

            return isDeleted
                ? Success<string>(_localizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }

    }
    #endregion
}
