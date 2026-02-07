using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Brand.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.Brand.Commands.Handlers
{
    public class BrandCommandHandler : ResponseHandler,
                        IRequestHandler<CreateBrandCommand, Response<string>>,
                        IRequestHandler<EditBrandCommand, Response<string>>,
                        IRequestHandler<DeleteBrandCommand, Response<string>>
    {
        #region Fields
        private readonly IBrandService _brandService;
        private readonly IProductService _productService;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public BrandCommandHandler(IBrandService brandService, ICurrentUserService currentUserService,
            IFileService fileService,
            IMapper mapper, IProductService productService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _brandService = brandService;
            _productService = productService;
            _fileService = fileService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<string>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            if (request.Image == null)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.NoImagesProvided]);

            var url = await _fileService.UploadImageAsync(request.Image, "brand-images");

            if (url == null)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToUploadImage]);

            var brand = new DataAccess.Entities.Brand
            {
                NameEn = request.NameEn,
                NameAr = request.NameAr,
                ImageUrl = url,
                CreatedBy = currentUser.UserName
            };

            var isSuccess = await _brandService.AddAsync(brand);

            if (isSuccess)
                return Created("");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditBrandCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // Check if brand exists
            var existingBrand = await _brandService.GetByIdAsync(request.Id);
            if (existingBrand == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // لو في صورة جديدة
            if (request.Image != null)
            {
                var newUrl = await _fileService.ReplaceImageAsync(
                    existingBrand.ImageUrl,
                    request.Image,
                    "brand-images");

                existingBrand.ImageUrl = newUrl;
            }

            // تحديث باقي البيانات
            existingBrand.NameEn = request.NameEn;
            existingBrand.NameAr = request.NameAr;
            existingBrand.UpdatedAt = DateTime.UtcNow;
            existingBrand.UpdatedBy = currentUser.UserName;

            var isSuccess = await _brandService.EditAsync(existingBrand);

            if (isSuccess)
                return Success<string>(_stringLocalizer[SharedResourcesKeys.Updated]);
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // Check if brand exists
            var existingBrand = await _brandService.GetByIdAsync(request.Id);
            if (existingBrand == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Check if brand has related products
            var hasProducts = await _productService.IsAnyProductRelatedToBrandAsync(request.Id);
            if (hasProducts)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.RelatedProducts]);

            // حذف الصورة من السيرفر
            _fileService.DeleteImage(existingBrand.ImageUrl);

            // Soft delete
            existingBrand.IsDeleted = true;
            existingBrand.UpdatedAt = DateTime.UtcNow;
            existingBrand.UpdatedBy = currentUser.UserName;

            var isSuccess = await _brandService.EditAsync(existingBrand);

            return isSuccess
                ? Success<string>(_stringLocalizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }
        #endregion
    }
}
