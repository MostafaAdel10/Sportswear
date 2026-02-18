using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.CartItem.Queries.Models;
using Sportswear.Core.Features.CartItem.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.CartItem.Queries.Handlers
{
    public class CartItemQueryHandler : ResponseHandler,
        IRequestHandler<GetCartItemsListQuery, Response<List<CartItemDto>>>,
        IRequestHandler<GetCartItemByIdQuery, Response<CartItemDto>>,
        IRequestHandler<GetCartSummaryQuery, Response<CartSummaryDto>>
    {
        #region Fields
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public CartItemQueryHandler(
            ICartService cartService,
            ICartItemService cartItemService,
            IProductService productService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer
        ) : base(localizer)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
            _productService = productService;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _mapper = mapper;
        }
        #endregion

        #region Get Cart Items
        public async Task<Response<List<CartItemDto>>> Handle(GetCartItemsListQuery request, CancellationToken cancellationToken)
        {
            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var userId = _currentUserService.GetUserId();

            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null)
                return NotFound<List<CartItemDto>>(_localizer[SharedResourcesKeys.CartItemNotFound]);

            var items = cart.Items
                .Select(x =>

                new CartItemDto
                {
                    Id = x.Id,
                    ProductVariantId = x.ProductVariantId,
                    Size = x.ProductVariant.Size,
                    ColorName = x.ProductVariant.ColorName,
                    ColorHex = x.ProductVariant.ColorHex,

                    ProductName = isArabic ? x.ProductVariant.Product.NameAr : x.ProductVariant.Product.NameEn,
                    ProductImageUrl = x.ProductVariant.Product.Images.FirstOrDefault()?.Url, // الصورة الأولى فقط

                    OriginalPrice = x.ProductVariant.Price > 0 ? x.ProductVariant.Price : x.ProductVariant.Product.BasePrice,
                    FinalPrice = _productService.CalculateDiscountedPriceOnProductVariants(x.ProductVariant.Product, x.ProductVariant.Price)
                            ?? x.ProductVariant.Price,  // الخصم على سعر المتغير
                    Quantity = x.Quantity
                })
                .ToList();

            return Success(items);
        }
        #endregion

        #region Get Cart Item By Id
        public async Task<Response<CartItemDto>> Handle(GetCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var item = await _cartItemService.GetByIdWithIncludesAsync(request.Id);
            if (item == null)
                return NotFound<CartItemDto>(_localizer[SharedResourcesKeys.CartItemNotFound]);

            var userId = _currentUserService.GetUserId();
            if (!await _cartService.IsCartOwnedByUser(item.CartId, userId))
                return Unauthorized<CartItemDto>(_localizer[SharedResourcesKeys.UnAuthorized]);

            var dto = new CartItemDto
            {
                Id = item.Id,
                ProductVariantId = item.ProductVariantId,
                Size = item.ProductVariant.Size,
                ColorName = item.ProductVariant.ColorName,
                ColorHex = item.ProductVariant.ColorHex,

                ProductName = isArabic ? item.ProductVariant.Product.NameAr : item.ProductVariant.Product.NameEn,
                ProductImageUrl = item.ProductVariant.Product.Images.FirstOrDefault()?.Url, // الصورة الأولى فقط

                OriginalPrice = item.ProductVariant.Price > 0 ? item.ProductVariant.Price : item.ProductVariant.Product.BasePrice,
                FinalPrice = _productService.CalculateDiscountedPriceOnProductVariants(item.ProductVariant.Product, item.ProductVariant.Price)
                            ?? item.ProductVariant.Price,  // الخصم على سعر المتغير
                Quantity = item.Quantity
            };

            return Success(dto);
        }
        #endregion

        #region Cart Summary
        public async Task<Response<CartSummaryDto>> Handle(GetCartSummaryQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();

            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null)
                return NotFound<CartSummaryDto>(_localizer[SharedResourcesKeys.CartNotFound]);

            var totalItems = cart.Items.Sum(x => x.Quantity);

            var totalPrice = cart.Items.Sum(x =>
            {
                var price = x.ProductVariant.Price > 0 ? x.ProductVariant.Price : x.ProductVariant.Product.BasePrice;
                return price * x.Quantity;
            });

            var summary = new CartSummaryDto
            {
                TotalItems = totalItems,
                TotalPrice = totalPrice
            };

            return Success(summary);
        }
        #endregion

    }
}
