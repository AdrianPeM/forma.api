namespace Forma.Api.Constants
{
    /// <summary>
    /// Centralized error codes for the API.
    /// Use these constants instead of hardcoded strings throughout your application.
    /// </summary>
    public static class ErrorCodes
    {
        // Generic validation errors
        public const string Required = "required";
        public const string Invalid = "invalid";
        public const string InvalidFormat = "invalid_format";
        public const string InvalidType = "invalid_type";
        public const string TooShort = "too_short";
        public const string TooLong = "too_long";
        public const string TooSmall = "too_small";
        public const string TooLarge = "too_large";
        public const string OutOfRange = "out_of_range";
        public const string Mismatch = "mismatch";

        // Authentication & Authorization
        public const string InvalidCredentials = "invalid_credentials";
        public const string Unauthorized = "unauthorized";
        public const string Forbidden = "forbidden";
        public const string TokenExpired = "token_expired";
        public const string TokenInvalid = "token_invalid";

        // User-specific errors
        public const string EmailAlreadyExists = "already_exists";
        public const string UserNotFound = "not_found";
        public const string UserInactive = "inactive";
        public const string UserBlocked = "blocked";

        // Resource errors
        public const string NotFound = "not_found";
        public const string AlreadyExists = "already_exists";
        public const string Conflict = "conflict";

        // Helper method to build full error code
        public static string ForField(string field, string errorCode)
        {
            return $"error.{field.ToLower()}.{errorCode}";
        }
    }
}