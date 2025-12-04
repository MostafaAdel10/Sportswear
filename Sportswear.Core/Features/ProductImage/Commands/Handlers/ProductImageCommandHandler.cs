using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ProductImage.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.ProductImage.Commands.Handlers
{
    public class ProductImageCommandHandler : ResponseHandler,
                        IRequestHandler<AddProductImagesCommand, Response<string>>,
                        IRequestHandler<AddProductImageCommand, Response<string>>,
                        IRequestHandler<EditProductImageCommand, Response<string>>,
                        IRequestHandler<DeleteProductImageCommand, Response<string>>
    {
        #region Fields
        private readonly IProductImageService _productImageService;
        private readonly IProductService _productService;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ProductImageCommandHandler(IProductImageService productImageService,
                                     IProductService productService,
                                     IFileService fileService,
                                     ICurrentUserService currentUserService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productImageService = productImageService;
            _productService = productService;
            _fileService = fileService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddProductImagesCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            if (request.Images == null || !request.Images.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.NoImagesProvided]);

            try
            {
                var urls = await _fileService.UploadImagesAsync(request.Images, "product-images");

                if (urls == null) return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToUploadImage]);


                var productImages = urls.Select(url => new DataAccess.Entities.ProductImage
                {
                    ProductId = request.ProductId,
                    Url = url,
                    CreatedBy = currentUser.UserName,
                }).ToList();

                var result = await _productImageService.AddRangeAsync(productImages);
                if (!result)
                    return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToUploadImage]);

                return Success<string>(_localizer[SharedResourcesKeys.ImagesUploadedSuccessfully]);
            }
            catch (ValidationException ex)
            {
                return BadRequest<string>(ex.Message);
            }
        }

        public async Task<Response<string>> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            if (request.Image == null)
                return BadRequest<string>(_localizer[SharedResourcesKeys.NoImagesProvided]);

            try
            {
                var url = await _fileService.UploadImageAsync(request.Image, "product-images");

                if (url == null) return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToUploadImage]);


                var productImage = new DataAccess.Entities.ProductImage
                {
                    ProductId = request.ProductId,
                    Url = url,
                    CreatedBy = currentUser.UserName,
                };

                var result = await _productImageService.AddAsync(productImage);
                if (!result)
                    return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToUploadImage]);

                return Success<string>(_localizer[SharedResourcesKeys.ImagesUploadedSuccessfully]);
            }
            catch (ValidationException ex)
            {
                return BadRequest<string>(ex.Message);
            }
        }

        public async Task<Response<string>> Handle(EditProductImageCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            var oldImage = await _productImageService.GetImageByProductIdAndImageUrlAsync(request.ProductId, request.OldImageUrl);
            if (oldImage == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.NoImagesProvided]);

            try
            {
                var newUrl = await _fileService.ReplaceImageAsync(request.OldImageUrl, request.NewImage, "product-images");

                if (newUrl == null) return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToUploadImage]);

                oldImage.Url = newUrl;
                oldImage.UpdatedAt = DateTime.UtcNow;
                oldImage.UpdatedBy = currentUser.UserName;

                var result = await _productImageService.EditAsync(oldImage);
                if (!result)
                    return BadRequest<string>(_localizer[SharedResourcesKeys.UpdateFailed]);

                return Success<string>(_localizer[SharedResourcesKeys.ImagesUploadedSuccessfully]);
            }
            catch (ValidationException ex)
            {
                return BadRequest<string>(ex.Message);
            }
        }

        public async Task<Response<string>> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.ProductNotFound]);

            var image = await _productImageService.GetImageByProductIdAndImageUrlAsync(request.ProductId, request.ImageUrl);
            if (image == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.NoImageToDelete]);

            try
            {
                var deleted = _fileService.DeleteImage(request.ImageUrl);
                if (!deleted)
                    return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToDelete]);

                var result = await _productImageService.DeleteAsync(image);
                if (!result)
                    return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToDelete]);

                return Success<string>(_localizer[SharedResourcesKeys.Deleted]);
            }
            catch (ValidationException ex)
            {
                return BadRequest<string>(ex.Message);
            }
        }

        #endregion
    }
}
