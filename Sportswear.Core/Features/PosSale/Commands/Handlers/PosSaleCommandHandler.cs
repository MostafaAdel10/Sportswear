using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.PosSale.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.DataAccess.Entities;
using Sportswear.DataAccess.Enums;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.PosSale.Commands.Handlers
{
    public class PosSaleCommandHandler : ResponseHandler,
        IRequestHandler<CreatePosSaleCommand, Response<int>>,
        IRequestHandler<CancelPosSaleCommand, Response<string>>
    {
        #region Fields
        private readonly IPosSaleService _posSaleService;
        private readonly IProductVariantService _productVariantService;
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public PosSaleCommandHandler(
            IPosSaleService posSaleService,
            IProductVariantService productVariantService,
            IProductService productService,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _posSaleService = posSaleService;
            _productVariantService = productVariantService;
            _productService = productService;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<int>> Handle(CreatePosSaleCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null)
                return Unauthorized<int>();

            // 1️⃣ جيب كل الـ variants مرة واحدة ✅
            var variantIds = request.Items.Select(i => i.ProductVariantId).ToList();
            var variants = await _productVariantService.GetByIdsWithProductAsync(variantIds);

            // 2️⃣ تحقق من الـ Stock
            foreach (var item in request.Items)
            {
                var variant = variants.FirstOrDefault(v => v.Id == item.ProductVariantId);
                if (variant == null)
                    return NotFound<int>(_localizer[SharedResourcesKeys.NotFound]);

                if (variant.StockQuantity < item.Quantity)
                    return BadRequest<int>(_localizer[SharedResourcesKeys.InsufficientStock]);
            }

            // 3️⃣ احسب الـ Total
            decimal totalAmount = 0;
            decimal discountAmount = 0;
            var saleItems = new List<PosSaleItem>();

            foreach (var item in request.Items)
            {
                var variant = variants.First(v => v.Id == item.ProductVariantId);
                var originalPrice = variant.Price;

                var discountedPrice = _productService
                    .CalculateDiscountedPriceOnProductVariants(variant.Product, originalPrice)
                    ?? originalPrice;

                var itemDiscount = (originalPrice - discountedPrice) * item.Quantity;
                var itemTotal = discountedPrice * item.Quantity;

                discountAmount += itemDiscount;
                totalAmount += originalPrice * item.Quantity;

                saleItems.Add(new PosSaleItem
                {
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    UnitPrice = originalPrice,
                    DiscountAmount = itemDiscount,
                    TotalPrice = itemTotal
                });
            }

            decimal finalAmount = totalAmount - discountAmount;

            // 4️⃣ Generate Sale Number
            var saleNumber = await _posSaleService.GenerateSaleNumberAsync();

            // 5️⃣ Create PosSale
            var posSale = new DataAccess.Entities.PosSale
            {
                SaleNumber = saleNumber,
                SaleDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                PaymentMethod = request.PaymentMethod,
                Status = PosSaleStatus.Completed,
                Notes = request.Notes,
                CreatedBy = currentUser.UserName,
                Items = saleItems
            };

            var saleId = await _posSaleService.AddAsync(posSale);
            if (saleId <= 0)
                return BadRequest<int>();

            // 6️⃣ خصم من الـ Stock
            foreach (var item in request.Items)
            {
                var variant = variants.First(v => v.Id == item.ProductVariantId);
                variant.StockQuantity -= item.Quantity;
                await _productVariantService.EditStockOnlyAsync(variant);
            }

            return Success(saleId, _localizer[SharedResourcesKeys.Created]);
        }

        public async Task<Response<string>> Handle(
            CancelPosSaleCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ جيب الـ Sale
            var posSale = await _posSaleService.GetByIdWithItemsAsync(request.Id);
            if (posSale == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.PosSaleNotFound]);

            // 2️⃣ تحقق إنها مش Cancelled
            if (posSale.Status == PosSaleStatus.Cancelled)
                return BadRequest<string>(_localizer[SharedResourcesKeys.OrderCannotBeCanceled]);

            // 3️⃣ غير الـ Status
            posSale.Status = PosSaleStatus.Cancelled;
            posSale.IsDeleted = true;
            await _posSaleService.EditAsync(posSale);

            // 4️⃣ رجّع الـ Stock
            foreach (var item in posSale.Items)
            {
                var variant = item.ProductVariant;
                variant.StockQuantity += item.Quantity;
                await _productVariantService.EditStockOnlyAsync(variant);
            }

            return Success<string>(_localizer[SharedResourcesKeys.CanceledOrder]);
        }
        #endregion
    }
}
