using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ProductAttributeTemplate.Queries.Models;
using Sportswear.Core.Features.ProductAttributeTemplate.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.ProductAttributeTemplate.Queries.Handlers
{
    public class ProductAttributeTemplateQueryHandler : ResponseHandler,
        IRequestHandler<GetAttributeTemplatesByCategoryIdQuery, Response<List<AttributeTemplateResponse>>>
    {
        #region Fields
        private readonly IProductAttributeTemplateService _templateService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ProductAttributeTemplateQueryHandler(
            IProductAttributeTemplateService templateService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _templateService = templateService;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<List<AttributeTemplateResponse>>> Handle(GetAttributeTemplatesByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var templates = await _templateService.GetByCategoryIdAsync(request.CategoryId);

            var response = templates.Select(t => new AttributeTemplateResponse
            {
                Id = t.Id,
                KeyEn = t.KeyEn,
                KeyAr = t.KeyAr,
                Type = t.Type.ToString()
            }).ToList();

            return Success(response);
        }
        #endregion
    }
}
