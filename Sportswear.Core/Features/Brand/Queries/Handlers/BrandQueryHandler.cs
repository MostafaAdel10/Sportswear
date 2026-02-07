using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Brand.Queries.Models;
using Sportswear.Core.Features.Brand.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

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
        #endregion

        #region Constructors
        public BrandQueryHandler(IBrandService brandService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _brandService = brandService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetBrandsListResponse>>> Handle(GetBrandsListQuery request, CancellationToken cancellationToken)
        {
            var brandsList = await _brandService.GetBrandsListAsync();
            var brandsListMapper = _mapper.Map<List<GetBrandsListResponse>>(brandsList);

            var result = Success(brandsListMapper);
            result.Meta = new { Count = brandsListMapper.Count() };
            return result;
        }

        public async Task<Response<GetBrandByIdResponse>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandService.GetByIdAsync(request.Id);

            if (brand is null)
                return NotFound<GetBrandByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetBrandByIdResponse>(brand);

            return Success(result);
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
