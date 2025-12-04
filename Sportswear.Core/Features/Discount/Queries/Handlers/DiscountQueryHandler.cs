using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Discount.Queries.Models;
using Sportswear.Core.Features.Discount.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Discount.Queries.Handlers
{
    public class DiscountQueryHandler : ResponseHandler,
        IRequestHandler<GetActiveDiscountsQuery, Response<List<GetActiveDiscountsResponse>>>,
        IRequestHandler<GetActiveDiscountByIdQuery, Response<GetDiscountByIdResponse>>
    {
        #region Fields
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public DiscountQueryHandler(IDiscountService discountService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _discountService = discountService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetActiveDiscountsResponse>>> Handle(GetActiveDiscountsQuery request, CancellationToken cancellationToken)
        {
            var discounts = await _discountService.GetActiveDiscountsAsync();
            var mappedDiscounts = _mapper.Map<List<GetActiveDiscountsResponse>>(discounts);

            var result = Success(mappedDiscounts);
            result.Meta = new { Count = mappedDiscounts.Count() };
            return result;
        }

        public async Task<Response<GetDiscountByIdResponse>> Handle(GetActiveDiscountByIdQuery request, CancellationToken cancellationToken)
        {
            var discount = await _discountService.GetActiveDiscountByIdAsync(request.Id);

            if (discount is null)
                return NotFound<GetDiscountByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetDiscountByIdResponse>(discount);

            return Success(result);
        }
        #endregion
    }
}
