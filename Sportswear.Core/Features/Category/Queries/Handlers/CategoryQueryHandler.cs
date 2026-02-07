using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Category.Queries.Models;
using Sportswear.Core.Features.Category.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

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
        #endregion

        #region Constructors
        public CategoryQueryHandler(ICategoryService categoryService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetCategoriesListResponse>>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
        {
            var categoriesList = await _categoryService.GetCategoriesListAsync();
            var categoriesListMapper = _mapper.Map<List<GetCategoriesListResponse>>(categoriesList);

            var result = Success(categoriesListMapper);
            result.Meta = new { Count = categoriesListMapper.Count() };
            return result;
        }

        public async Task<Response<GetCategoryByIdResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(request.Id);

            if (category is null)
                return NotFound<GetCategoryByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetCategoryByIdResponse>(category);

            return Success(result);
        }

        public async Task<Response<GetCategoryByIdToEditResponse>> Handle(GetCategoryByIdToEditQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(request.Id);

            if (category is null)
                return NotFound<GetCategoryByIdToEditResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetCategoryByIdToEditResponse>(category);

            return Success(result);
        }
        #endregion
    }
}
