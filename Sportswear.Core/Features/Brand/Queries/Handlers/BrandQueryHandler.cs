using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Brand.Queries.Models;
using Sportswear.Core.Features.Brand.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.Implementations;

namespace Sportswear.Core.Features.Brand.Queries.Handlers
{
    public class BrandQueryHandler : ResponseHandler,
        IRequestHandler<GetBrandsListQuery, Response<List<GetBrandsListResponse>>>,
        IRequestHandler<GetBrandByIdQuery, Response<GetBrandByIdResponse>>,
        IRequestHandler<GetBrandByIdToEditQuery, Response<GetBrandByIdToEditResponse>>
    {
        #region Fields
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly ICacheService _cacheService;
        #endregion

        #region Constructors
        public BrandQueryHandler(IBrandService brandService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer, ICacheService cacheService) : base(stringLocalizer)
        {
            _brandService = brandService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _cacheService = cacheService;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetBrandsListResponse>>> Handle(GetBrandsListQuery request, CancellationToken cancellationToken)
        {
            // 1. Check Cache
            var cached = _cacheService.Get<List<GetBrandsListResponse>>(CacheKeys.Brands);
            if (cached != null)
                return Success(cached);

            // 2. جيب من DB
            var brandsList = await _brandService.GetBrandsListAsync();
            var mapped = _mapper.Map<List<GetBrandsListResponse>>(brandsList);

            // 3. احفظ في Cache لمدة 60 دقيقة
            _cacheService.Set(CacheKeys.Brands, mapped, TimeSpan.FromMinutes(60));

            var result = Success(mapped);
            result.Meta = new { Count = mapped.Count() };
            return result;
        }

        public async Task<Response<GetBrandByIdResponse>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CacheKeys.BrandById, request.Id);

            // 1. Check Cache
            var cached = _cacheService.Get<GetBrandByIdResponse>(cacheKey);
            if (cached != null)
                return Success(cached);

            // 2. جيب من DB
            var brand = await _brandService.GetByIdAsync(request.Id);
            if (brand is null)
                return NotFound<GetBrandByIdResponse>(
                    _stringLocalizer[SharedResourcesKeys.NotFound]);

            var mapped = _mapper.Map<GetBrandByIdResponse>(brand);

            // 3. احفظ في Cache
            _cacheService.Set(cacheKey, mapped, TimeSpan.FromMinutes(60));

            return Success(mapped);
        }

        public async Task<Response<GetBrandByIdToEditResponse>> Handle(GetBrandByIdToEditQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandService.GetByIdAsync(request.Id);

            if (brand is null)
                return NotFound<GetBrandByIdToEditResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetBrandByIdToEditResponse>(brand);

            return Success(result);
        }
        #endregion
    }
}
