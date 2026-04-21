namespace Sportswear.Api.Helper
{
    public static class RateLimitingPolicies
    {
        public const string Login = "login";
        public const string Register = "register";
        public const string Api = "api";
        public const string Upload = "upload";
        public const string ResetPassword = "reset_password";
        public const string Order = "order";
        public const string Review = "review";
    }
}
