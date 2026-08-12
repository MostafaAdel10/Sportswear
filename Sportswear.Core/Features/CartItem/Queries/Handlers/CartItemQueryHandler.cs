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
        #endregion

        #region Constructor
        public CartItemQueryHandler(
            ICartService cartService,
            ICartItemService cartItemService,
            IProductService productService,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer
        ) : base(localizer)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
            _productService = productService;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<List<CartItemDto>>> Handle(GetCartItemsListQuery request, CancellationToken cancellationToken)
        {
            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var userId = _currentUserService.GetUserId();

            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null)
                return NotFound<List<CartItemDto>>(_localizer[SharedResourcesKeys.CartItemNotFound]);

            var items = cart.Items.Select(x =>
            {
                var finalPrice = _productService.CalculateDiscountedPriceOnProductVariants(
                    x.ProductVariant.Product, x.ProductVariant.Price) ?? x.ProductVariant.Price;

                return new CartItemDto
                {
                    Id = x.Id,
                    ProductVariantId = x.ProductVariantId,
                    SKU = x.ProductVariant.SKU,
                    ProductName = isArabic
                        ? x.ProductVariant.Product.NameAr
                        : x.ProductVariant.Product.NameEn,
                    AttributeKey = isArabic
                        ? x.ProductVariant.Product.AttributeKeyAr
                        : x.ProductVariant.Product.AttributeKeyEn,
                    ProductImageUrl = x.ProductVariant.Product.Images.FirstOrDefault()?.Url,
                    OriginalPrice = x.ProductVariant.Price,
                    FinalPrice = finalPrice,
                    Quantity = x.Quantity,
                    StockQuantity = x.ProductVariant.StockQuantity,
                    TotalPrice = finalPrice * x.Quantity,
                    AttributeValue = isArabic
                        ? x.ProductVariant.AttributeValueAr
                        : x.ProductVariant.AttributeValueEn,
                    Unit = x.ProductVariant.Unit,
                    ColorLabel = x.ProductVariant.ColorLabel,
                    ColorHex = x.ProductVariant.ColorHex
                };
            }).ToList();

            return Success(items);
        }

        public async Task<Response<CartItemDto>> Handle(GetCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var item = await _cartItemService.GetByIdWithIncludesAsync(request.Id);
            if (item == null)
                return NotFound<CartItemDto>(_localizer[SharedResourcesKeys.CartItemNotFound]);

            var userId = _currentUserService.GetUserId();
            if (!await _cartService.IsCartOwnedByUser(item.CartId, userId))
                return Unauthorized<CartItemDto>(_localizer[SharedResourcesKeys.UnAuthorized]);

            var finalPrice = _productService.CalculateDiscountedPriceOnProductVariants(
                item.ProductVariant.Product, item.ProductVariant.Price) ?? item.ProductVariant.Price;

            var dto = new CartItemDto
            {
                Id = item.Id,
                ProductVariantId = item.ProductVariantId,
                SKU = item.ProductVariant.SKU,
                ProductName = isArabic
                    ? item.ProductVariant.Product.NameAr
                    : item.ProductVariant.Product.NameEn,
                AttributeKey = isArabic
                        ? item.ProductVariant.Product.AttributeKeyAr
                        : item.ProductVariant.Product.AttributeKeyEn,
                ProductImageUrl = item.ProductVariant.Product.Images.FirstOrDefault()?.Url,
                OriginalPrice = item.ProductVariant.Price,
                FinalPrice = finalPrice,
                Quantity = item.Quantity,
                StockQuantity = item.ProductVariant.StockQuantity,
                TotalPrice = finalPrice * item.Quantity,
                AttributeValue = isArabic
                        ? item.ProductVariant.AttributeValueAr
                        : item.ProductVariant.AttributeValueEn,
                Unit = item.ProductVariant.Unit,
                ColorLabel = item.ProductVariant.ColorLabel,
                ColorHex = item.ProductVariant.ColorHex
            };

            return Success(dto);
        }

        public async Task<Response<CartSummaryDto>> Handle(GetCartSummaryQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();

            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null)
                return NotFound<CartSummaryDto>(_localizer[SharedResourcesKeys.CartNotFound]);

            var totalItems = cart.Items.Sum(x => x.Quantity);

            // ✅ حساب السعر الأصلي
            var totalOriginalPrice = cart.Items.Sum(x => x.ProductVariant.Price * x.Quantity);

            // ✅ حساب السعر بعد الخصم
            var totalFinalPrice = cart.Items.Sum(x =>
            {
                var finalPrice = _productService.CalculateDiscountedPriceOnProductVariants(
                    x.ProductVariant.Product, x.ProductVariant.Price) ?? x.ProductVariant.Price;
                return finalPrice * x.Quantity;
            });

            var summary = new CartSummaryDto
            {
                TotalItems = totalItems,
                TotalPrice = totalOriginalPrice,
                TotalPriceAfterDiscount = totalFinalPrice,
                TotalDiscount = totalOriginalPrice - totalFinalPrice
            };

            return Success(summary);
        }
        #endregion

    }
}
