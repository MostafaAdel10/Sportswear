using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.CartItem.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.DataAccess.Entities;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.CartItem.Commands.Handlers
{
    public class CartItemCommandHandler : ResponseHandler,
        IRequestHandler<AddCartItemCommand, Response<string>>,
        IRequestHandler<EditCartItemCommand, Response<string>>,
        IRequestHandler<DeleteCartItemCommand, Response<string>>
    {
        #region Fields
        private readonly ICartItemService _cartItemService;
        private readonly ICartService _cartService;
        private readonly IProductVariantService _variantService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public CartItemCommandHandler(ICartItemService cartItemService,
            ICartService cartService,
            IProductVariantService variantService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _cartItemService = cartItemService;
            _cartService = cartService;
            _variantService = variantService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<string>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            // 1) Check Variant Exists
            var variant = await _variantService.GetByIdAsync(request.ProductVariantId);
            if (variant == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.VariantNotFound]);

            // 2) Check Stock
            if (request.Quantity <= 0 || request.Quantity > variant.StockQuantity)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvalidQuantity]);

            // 3) Get User Cart
            var userId = _currentUserService.GetUserId();

            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null)
            {
                await _cartService.AddAsync(new Cart
                {
                    UserId = userId
                });
                cart = await _cartService.GetByUserIdAsync(userId);
            }

            // 4) Check if item already exists (increase quantity)
            var existItem = await _cartItemService.GetCartItemByCartAndVariant(cart.Id, request.ProductVariantId);
            if (existItem != null)
            {
                existItem.Quantity += request.Quantity;

                if (existItem.Quantity > variant.StockQuantity)
                    return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.StockExceeded]);

                await _cartItemService.EditAsync(existItem);
                return Success<string>(_stringLocalizer[SharedResourcesKeys.CartItemUpdated]);
            }

            // 5) Add new item
            var isAdded = await _cartItemService.AddAsync(new DataAccess.Entities.CartItem
            {
                CartId = cart.Id,
                ProductVariantId = request.ProductVariantId,
                Quantity = request.Quantity
            });

            return isAdded ? Created("") : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditCartItemCommand request, CancellationToken cancellationToken)
        {
            // 1) Get Cart Item
            var cartItem = await _cartItemService.GetByIdAsync(request.Id);
            if (cartItem == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.CartItemNotFound]);

            // 2) Ensure User Owns Cart
            var userId = _currentUserService.GetUserId();
            if (!await _cartService.IsCartOwnedByUser(cartItem.CartId, userId))
                return Unauthorized<string>(_stringLocalizer[SharedResourcesKeys.UnAuthorized]);

            // 3) Validate Quantity
            var variant = await _variantService.GetByIdAsync(cartItem.ProductVariantId);
            if (variant == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.VariantNotFound]);

            if (request.Quantity <= 0 || request.Quantity > variant.StockQuantity)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvalidQuantity]);

            // 4) Update
            cartItem.Quantity = request.Quantity;
            var isUpdated = await _cartItemService.EditAsync(cartItem);

            return isUpdated ? Success<string>(_stringLocalizer[SharedResourcesKeys.CartItemUpdated]) : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            var cartItem = await _cartItemService.GetByIdAsync(request.Id);
            if (cartItem == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.CartItemNotFound]);

            var userId = _currentUserService.GetUserId();
            if (!await _cartService.IsCartOwnedByUser(cartItem.CartId, userId))
                return Unauthorized<string>(_stringLocalizer[SharedResourcesKeys.UnAuthorized]);

            var isDeleted = await _cartItemService.DeleteAsync(cartItem);

            return isDeleted ? Deleted<string>(_stringLocalizer[SharedResourcesKeys.Deleted]) : BadRequest<string>();
        }

        #endregion
    }
}
