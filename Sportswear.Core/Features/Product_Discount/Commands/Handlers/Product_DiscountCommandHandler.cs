using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product_Discount.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.Product_Discount.Commands.Handlers
{
    public class Product_DiscountCommandHandler : ResponseHandler,
                        IRequestHandler<AddDiscountToProductsCommand, Response<string>>,
                        IRequestHandler<UpdateProductsForDiscountCommand, Response<string>>,
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

            // التحقق من وجود الخصم
            var discount = await _discountService.GetByIdAsync(request.DiscountId);
            if (discount == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.DiscountNotExist]);

            // التحقق من المنتجات وجمعها
            var products = await _productService.GetByIdsAsync(request.ProductIds);
            if (products.Count != request.ProductIds.Count)
                return BadRequest<string>(_localizer[SharedResourcesKeys.SomeProductsNotFound]);

            // إنشاء الروابط الجديدة، مع تجنب التكرار
            var newLinks = new List<DataAccess.Entities.Product_Discount>();
            foreach (var product in products)
            {
                if (await _product_DiscountService.ExistsAsync(request.DiscountId, product.Id))
                    continue; // تجنب إضافة مكرر

                newLinks.Add(new DataAccess.Entities.Product_Discount
                {
                    DiscountId = request.DiscountId,
                    ProductId = product.Id
                });
            }

            if (!newLinks.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.DiscountAlreadyApplied]);

            // إضافة batch
            var isAdded = await _product_DiscountService.AddRangeAsync(newLinks);

            return isAdded ? Created("") : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(UpdateProductsForDiscountCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var discount = await _discountService.GetByIdAsync(request.DiscountId);
            if (discount == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.DiscountNotExist]);

            // جلب الروابط الحالية
            var existingLinks = await _product_DiscountService.GetByDiscountIdAsync(request.DiscountId);

            // إزالة الروابط القديمة
            if (existingLinks.Any())
            {
                await _product_DiscountService.DeleteRangeAsync(existingLinks);
            }

            // إضافة الروابط الجديدة (مثل الإضافة السابقة)
            var products = await _productService.GetByIdsAsync(request.NewProductIds);
            if (products.Count != request.NewProductIds.Count)
                return BadRequest<string>(_localizer[SharedResourcesKeys.SomeProductsNotFound]);

            var newLinks = products.Select(p => new DataAccess.Entities.Product_Discount
            {
                DiscountId = request.DiscountId,
                ProductId = p.Id
            }).ToList();

            // إضافة batch
            var isAdded = await _product_DiscountService.AddRangeAsync(newLinks);

            return isAdded ? Success<string>(_localizer[SharedResourcesKeys.Updated]) : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(RemoveDiscountFromProductsCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var discount = await _discountService.GetByIdAsync(request.DiscountId);
            if (discount == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.DiscountNotExist]);

            // جلب الروابط المراد حذفها
            var linksToDelete = await _product_DiscountService.GetByDiscountAndProductsAsync(request.DiscountId, request.ProductIds);

            if (!linksToDelete.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.NoLinksToDelete]);

            // حذف batch
            var isDeleted = await _product_DiscountService.DeleteRangeAsync(linksToDelete);
            return isDeleted ? Success<string>(_localizer[SharedResourcesKeys.Deleted]) : BadRequest<string>();
        }
    }
    #endregion
}
