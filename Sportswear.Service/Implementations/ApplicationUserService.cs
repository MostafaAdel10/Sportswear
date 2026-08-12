using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Sportswear.DataAccess.Entities.Identity;
using Sportswear.Infrastructure.Context;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        #region Fields
        private readonly ApplicationDbContext _applicationDBContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailsService _emailsService;
        private readonly IUrlHelper _urlHelper;
        #endregion

        #region Contractors
        public ApplicationUserService(UserManager<ApplicationUser> userManager,
            ApplicationDbContext applicationDBContext,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelper urlHelper,
            IEmailsService emailsService)
        {
            _userManager = userManager;
            _applicationDBContext = applicationDBContext;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
            _emailsService = emailsService;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddUserAsync(ApplicationUser user, string password)
        {
            var trans = await _applicationDBContext.Database.BeginTransactionAsync();
            try
            {
                //if Email is Exist
                var existUser = await _userManager.FindByEmailAsync(user.Email);
                //email is Exist
                if (existUser != null) return "EmailIsExist";

                //if username is Exist
                var userByUserName = await _userManager.FindByNameAsync(user.UserName);
                //username is Exist
                if (userByUserName != null) return "UserNameIsExist";
                //Create
                var createResult = await _userManager.CreateAsync(user, password);
                //Failed
                if (!createResult.Succeeded)
                    return string.Join(",", createResult.Errors.Select(x => x.Description).ToList());

                await _userManager.AddToRoleAsync(user, "User");

                // Send Confirm Email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var requestAccessor = _httpContextAccessor.HttpContext.Request;
                var returnUrl = requestAccessor.Scheme + "://" + requestAccessor.Host +
                    _urlHelper.Action("ConfirmEmail", "Authentication",
                        new { userId = user.Id, code = code });

                var message = $"To Confirm Email Click Link: <a href='{returnUrl}'>Link Of Confirmation</a>";

                await _emailsService.SendEmailAsync(user.Email, "Confirm Email ✅", message);

                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                Log.Error(ex, ex.ToString());

                return ex.ToString();
            }
        }
        #endregion
    }
}
