namespace GreenMileSharing.TripApi.Application.Helpers;

internal static class ApiEndpoints
{
    private const string ApiBase = "/api";

    internal static class Employee
    {
        private const string Base = $"{ApiBase}/employees";

        public const string GetAll = Base;
        
        public const string GetById = $"{Base}/{{id:guid}}";
        
        public const string GetByUsername = $"{Base}/{{username}}";
        
        public const string UpdateUsername = $"{Base}/{{id:guid}}";
        
        public const string DeleteById = $"{Base}/{{id:guid}}";
    }
    
    internal static class Car
    {
        private const string Base = $"{ApiBase}/cars";

        public const string Create = Base;
        
        public const string GetAll = Base;
        
        public const string Update = $"{Base}/{{id:guid}}";

        public const string DeleteById = $"{Base}/{{id:guid}}";
    }
    
    internal static class Trip
    {
        private const string Base = $"{ApiBase}/trips";

        public const string Create = Base;

        public const string DeleteById = $"{Base}/{{id:guid}}";
    }
}