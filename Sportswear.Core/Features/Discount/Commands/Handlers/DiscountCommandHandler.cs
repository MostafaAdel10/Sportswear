using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Discount.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Discount.Commands.Handlers
{
    public class DiscountCommandHandler : ResponseHandler,
                        IRequestHandler<CreateDiscountCommand, Response<string>>,
                        IRequestHandler<EditDiscountCommand, Response<string>>,
                        IRequestHandler<DeleteDiscountCommand, Response<string>>
    {
        #region Fields
        private readonly IDiscountService _discountService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public DiscountCommandHandler(IDiscountService discountService, IMapper mapper, IProductService productService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _discountService = discountService;
            _productService = productService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<string>> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = _mapper.Map<DataAccess.Entities.Discount>(request);

            discount.CreatedBy = "TestAdmin";

            var isSuccess = await _discountService.AddAsync(discount);

            if (isSuccess)
                return Created("");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditDiscountCommand request, CancellationToken cancellationToken)
        {
            // Check if Discount exists
            var existingDiscount = await _discountService.GetActiveDiscountByIdAsync(request.Id);
            if (existingDiscount == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Map new values to existing entity
            existingDiscount = _mapper.Map(request, existingDiscount);

            existingDiscount.UpdatedBy = "TestAdmin";
            existingDiscount.UpdatedAt = DateTime.UtcNow;

            var isSuccess = await _discountService.EditAsync(existingDiscount);

            if (isSuccess)
                return Success<string>(_stringLocalizer[SharedResourcesKeys.Updated]);
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            // Check if Discount exists
            var existingDiscount = await _discountService.GetActiveDiscountByIdAsync(request.Id);
            if (existingDiscount == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Check if Discount has related products
            var hasProducts = await _productService.IsAnyProductRelatedToDiscountAsync(request.Id);
            if (hasProducts)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.RelatedProducts]);

            // Soft delete
            existingDiscount.IsDeleted = true;
            existingDiscount.UpdatedAt = DateTime.UtcNow;
            existingDiscount.UpdatedBy = "TestAdmin";

            var isSuccess = await _discountService.EditAsync(existingDiscount);

            return isSuccess
                ? Success<string>(_stringLocalizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }
        #endregion
    }
}
