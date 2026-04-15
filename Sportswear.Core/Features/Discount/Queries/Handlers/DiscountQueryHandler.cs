using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Discount.Queries.Models;
using Sportswear.Core.Features.Discount.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.DataAccess.Enums;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Discount.Queries.Handlers
{
    public class DiscountQueryHandler : ResponseHandler,
        IRequestHandler<GetAllDiscountsQuery, Response<List<DiscountDto>>>,
        IRequestHandler<GetActiveDiscountsQuery, Response<List<GetActiveDiscountsResponse>>>,
        IRequestHandler<GetActiveDiscountByIdQuery, Response<GetDiscountByIdResponse>>,
        IRequestHandler<GetActiveDiscountByIdToEditQuery, Response<GetDiscountByIdToEditResponse>>
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
        public async Task<Response<List<DiscountDto>>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var discounts = await _discountService.GetAllWithProductsCountAsync();

            // ✅ فلتر حسب الـ Status
            var filtered = request.Status switch
            {
                DiscountStatusFilter.Active =>
                    discounts.Where(d => d.StartDate <= now && d.EndDate >= now).ToList(),

                DiscountStatusFilter.Expired =>
                    discounts.Where(d => d.EndDate < now).ToList(),

                DiscountStatusFilter.Upcoming =>
                    discounts.Where(d => d.StartDate > now).ToList(),

                _ => discounts // All
            };

            var data = filtered.Select(d =>
            {
                // ✅ تحديد الـ Status
                string status;
                int daysRemaining = 0;

                if (d.StartDate > now)
                {
                    status = "Upcoming";
                    daysRemaining = (int)(d.StartDate - now).TotalDays;
                }
                else if (d.EndDate < now)
                {
                    status = "Expired";
                    daysRemaining = 0;
                }
                else
                {
                    status = "Active";
                    daysRemaining = (int)(d.EndDate - now).TotalDays;
                }

                return new DiscountDto
                {
                    Id = d.Id,
                    Code = d.Code,
                    NameEn = d.NameEn,
                    NameAr = d.NameAr,
                    Percentage = d.Percentage,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    Status = status,
                    DaysRemaining = daysRemaining,
                    ProductsCount = d.Product_Discounts?.Count ?? 0,
                    Type = d.Type.ToString()
                };
            }).ToList();

            var result = Success(data);
            result.Meta = new
            {
                Total = data.Count,
                Active = data.Count(d => d.Status == "Active"),
                Expired = data.Count(d => d.Status == "Expired"),
                Upcoming = data.Count(d => d.Status == "Upcoming")
            };

            return result;
        }

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

        public async Task<Response<GetDiscountByIdToEditResponse>> Handle(GetActiveDiscountByIdToEditQuery request, CancellationToken cancellationToken)
        {
            var discount = await _discountService.GetActiveDiscountByIdAsync(request.Id);

            if (discount is null)
                return NotFound<GetDiscountByIdToEditResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetDiscountByIdToEditResponse>(discount);

            return Success(result);
        }
        #endregion
    }
}
