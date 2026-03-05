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
        private readonly IProductAttributeTemplateService _templateService;
        private readonly ISkuGeneratorService _skuGeneratorService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ProductVariantCommandHandler(
            IProductVariantService productVariantService,
            IProductService productService,
            IProductAttributeTemplateService templateService,
            ISkuGeneratorService skuGeneratorService,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _productVariantService = productVariantService;
            _productService = productService;
            _templateService = templateService;
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

            // جيب المنتج مع الـ Category
            var product = await _productService.GetByIdWithIncludesAsync(request.ProductId);
            if (product == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            // جيب الـ templates بتاعة الـ Category
            var templates = await _templateService.GetByCategoryIdAsync(product.CategoryId);

            // جيب الـ keys الموجودة للـ duplicate check
            var existingKeys = await _productVariantService.GetVariantKeysAsync(request.ProductId);

            var requestKeys = new HashSet<string>();
            var variants = new List<DataAccess.Entities.ProductVariant>();

            foreach (var dto in request.Variants)
            {
                // validate إن كل attribute موجود في الـ templates
                foreach (var attr in dto.Attributes)
                {
                    var template = templates.FirstOrDefault(t => t.Id == attr.TemplateId);
                    if (template == null)
                        return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidAttributeTemplate]);
                }

                // عمل key من الـ attributes بترتيب ثابت
                var key = string.Join("-", dto.Attributes
                    .OrderBy(a => a.TemplateId)
                    .Select(a => a.ValueEn.ToUpper()));

                // check duplicate في نفس الـ request
                if (!requestKeys.Add(key))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.ProductVariantDuplicateInRequest]);

                // check duplicate مع الموجود في الـ DB
                if (existingKeys.Contains(key))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.ProductVariantAlreadyExists]);

                // generate SKU
                var attributeValues = dto.Attributes
                    .OrderBy(a => a.TemplateId)
                    .Select(a => a.ValueEn)
                    .ToList();

                var sku = _skuGeneratorService.Generate(product.Code, attributeValues);

                variants.Add(new DataAccess.Entities.ProductVariant
                {
                    ProductId = request.ProductId,
                    SKU = sku,
                    Price = dto.Price > 0 ? dto.Price : product.BasePrice,
                    StockQuantity = dto.StockQuantity,
                    CreatedBy = currentUser.UserName,
                    Attributes = dto.Attributes.Select(a => new DataAccess.Entities.ProductVariantAttribute
                    {
                        ProductAttributeTemplateId = a.TemplateId,
                        ValueEn = a.ValueEn,
                        ValueAr = a.ValueAr,
                        ColorHex = a.ColorHex,
                        CreatedBy = currentUser.UserName,
                    }).ToList()
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
                return NotFound<string>(_localizer[SharedResourcesKeys.VariantNotFound]);

            // validate إن كل attribute موجود في الـ templates بتاعة الـ Category
            var templates = await _templateService.GetByCategoryIdAsync(variant.Product.CategoryId);
            foreach (var attr in request.Attributes)
            {
                var template = templates.FirstOrDefault(t => t.Id == attr.TemplateId);
                if (template == null)
                    return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidAttributeTemplate]);
            }

            // عمل key جديد من الـ attributes الجديدة
            var newKey = string.Join("-", request.Attributes
                .OrderBy(a => a.TemplateId)
                .Select(a => a.ValueEn.ToUpper()));

            // check duplicate مع باقي الـ variants غير الحالي
            var existingKeys = await _productVariantService.GetVariantKeysAsync(
                variant.ProductId, excludeId: request.Id);

            if (existingKeys.Contains(newKey))
                return BadRequest<string>(_localizer[SharedResourcesKeys.ProductVariantAlreadyExists]);

            // regenerate SKU
            var attributeValues = request.Attributes
                .OrderBy(a => a.TemplateId)
                .Select(a => a.ValueEn)
                .ToList();

            variant.SKU = _skuGeneratorService.Generate(variant.Product.Code, attributeValues);
            variant.Price = request.Price;
            variant.StockQuantity = request.StockQuantity;
            variant.UpdatedBy = currentUser.UserName;
            variant.UpdatedAt = DateTime.UtcNow;

            // امسح الـ attributes القديمة واحط الجديدة
            variant.Attributes = request.Attributes.Select(a => new DataAccess.Entities.ProductVariantAttribute
            {
                ProductVariantId = variant.Id,
                ProductAttributeTemplateId = a.TemplateId,
                ValueEn = a.ValueEn,
                ValueAr = a.ValueAr,
                ColorHex = a.ColorHex,
                CreatedBy = currentUser.UserName,
            }).ToList();

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

            // مش هينفع تحذف variant عنده orders
            if (variant.OrderItems != null && variant.OrderItems.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.CannotDeleteVariantWithOrders]);

            variant.IsDeleted = true;
            variant.UpdatedBy = currentUser.UserName;
            variant.UpdatedAt = DateTime.UtcNow;

            foreach (var attr in variant.Attributes)
            {
                attr.IsDeleted = true;
                attr.UpdatedBy = currentUser.UserName;
                attr.UpdatedAt = DateTime.UtcNow;
            }

            var isDeleted = await _productVariantService.EditAsync(variant);

            return isDeleted
                ? Success<string>(_localizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }
        #endregion
    }
}
