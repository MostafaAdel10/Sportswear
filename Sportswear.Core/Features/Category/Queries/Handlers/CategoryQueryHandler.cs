using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Category.Queries.Models;
using Sportswear.Core.Features.Category.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.Implementations;
using System.Globalization;
namespace Sportswear.Core.Features.Category.Queries.Handlers
{
    public class CategoryQueryHandler : ResponseHandler,
        IRequestHandler<GetCategoriesListQuery, Response<List<GetCategoriesListResponse>>>,
        IRequestHandler<GetCategoryByIdQuery, Response<GetCategoryByIdResponse>>,
        IRequestHandler<GetCategoryByIdToEditQuery, Response<GetCategoryByIdToEditResponse>>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly ICacheService _cacheService;
        #endregion
        #region Constructors
        public CategoryQueryHandler(ICategoryService categoryService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer, ICacheService cacheService) : base(stringLocalizer)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _cacheService = cacheService;
        }
        #endregion
        #region Handel Functions
        public async Task<Response<List<GetCategoriesListResponse>>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var cacheKey = $"{CacheKeys.Categories}_{culture}";
            // 1. Check Cache
            var cached = _cacheService.Get<List<GetCategoriesListResponse>>(cacheKey);
            if (cached != null)
                return Success(cached);
            // 2. Not found → Pocket from DB
            var categoriesList = await _categoryService.GetCategoriesListAsync();
            var mapped = _mapper.Map<List<GetCategoriesListResponse>>(categoriesList);
            // 3. Store in a cache for 60 minutes.
            _cacheService.Set(cacheKey, mapped, TimeSpan.FromMinutes(60));
            var result = Success(mapped);
            result.Meta = new { Count = mapped.Count() };
            return result;
        }
        public async Task<Response<GetCategoryByIdResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var cacheKey = $"{string.Format(CacheKeys.CategoryById, request.Id)}_{culture}";
            // 1. Check Cache
            var cached = _cacheService.Get<GetCategoryByIdResponse>(cacheKey);
            if (cached != null)
                return Success(cached);
            // 2. Not found → Pocket from DB
            var category = await _categoryService.GetByIdAsync(request.Id);
            if (category is null)
                return NotFound<GetCategoryByIdResponse>(
                    _stringLocalizer[SharedResourcesKeys.CategoryNotFound]);
            var mapped = _mapper.Map<GetCategoryByIdResponse>(category);
            // 3. Store in a cache for 60 minutes.
            _cacheService.Set(cacheKey, mapped, TimeSpan.FromMinutes(60));
            return Success(mapped);
        }
        public async Task<Response<GetCategoryByIdToEditResponse>> Handle(GetCategoryByIdToEditQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(request.Id);
            if (category is null)
                return NotFound<GetCategoryByIdToEditResponse>(_stringLocalizer[SharedResourcesKeys.CategoryNotFound]);
            var result = _mapper.Map<GetCategoryByIdToEditResponse>(category);
            return Success(result);
        }
        #endregion
    }
}