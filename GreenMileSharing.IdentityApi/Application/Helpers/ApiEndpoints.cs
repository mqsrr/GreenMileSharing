namespace GreenMileSharing.IdentityApi.Application.Helpers;

internal static class ApiEndpoints
{
    private const string ApiBase = "/api/v{apiVersion:apiVersion}";
    
    public static class Authentication
    {
        private const string Base = $"{ApiBase}/auth";

        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";
    }
}