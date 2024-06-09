namespace GreenMileSharing.Client.Helpers;

internal static class ApiEndpoints
{
    private const string ApiBase = "/api";
    
    public static class Authentication
    {
        private const string Base = $"{ApiBase}/auth";

        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";
    }
    
    internal static class Employee
    {
        private const string Base = $"{ApiBase}/employees";

        public const string GetById = $"{Base}/{{id}}";
        
        public const string GetByUsername = $"{Base}/{{username}}";
        
        public const string UpdateUsername = $"{Base}/{{id}}";
        
        public const string DeleteById = $"{Base}/{{id}}";
    }
    
    internal static class Car
    {
        private const string Base = $"{ApiBase}/cars";

        public const string Create = Base;
        
        public const string GetAll = Base;
        
        public const string Update = $"{Base}/{{id}}";

        public const string DeleteById = $"{Base}/{{id}}";
    }
    
    internal static class Trip
    {
        private const string Base = $"{ApiBase}/trips";

        public const string Create = Base;

        public const string DeleteById = $"{Base}/{{id}}";
    }
}