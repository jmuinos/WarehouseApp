using WarehouseApp.SharedKernel.Core.Primitives;

namespace WarehouseApp.SharedKernel.Core.Errors;

/// <summary>Contains the domain errors.</summary>
public static class DomainErrors
{
    /// <summary>Contains the user errors.</summary>
    public static class Company
    {
        public static Error NotFound => new Error("Company.NotFound", "The company with the specified identifier was not found.");
        
    }
    
    /// <summary>Contains the name errors.</summary>
    public static class Name
    {
        public static Error NullOrEmpty => new Error("Name.NullOrEmpty", "The name is required.");

        public static Error LongerThanAllowed => new Error("Name.LongerThanAllowed", "The name is longer than allowed.");
    }

    /// <summary>Contains the first name errors.</summary>
    public static class FirstName
    {
        public static Error NullOrEmpty => new Error("FirstName.NullOrEmpty", "The first name is required.");

        public static Error LongerThanAllowed => new Error("FirstName.LongerThanAllowed", "The first name is longer than allowed.");
    }

    /// <summary>Contains the last name errors.</summary>
    public static class LastName
    {
        public static Error NullOrEmpty => new Error("LastName.NullOrEmpty", "The last name is required.");

        public static Error LongerThanAllowed => new Error("LastName.LongerThanAllowed", "The last name is longer than allowed.");
    }

    /// <summary>Contains general errors.</summary>
    public static class General
    {
        public static Error UnProcessableRequest => new Error(
                                                              "General.UnProcessableRequest",
                                                              "The server could not process the request.");

        public static Error ServerError => new Error("General.ServerError", "The server encountered an unrecoverable error.");
    }
}