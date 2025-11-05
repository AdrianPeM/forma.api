using Microsoft.AspNetCore.Mvc.ModelBinding;
using Forma.Api.Responses;
using Forma.Api.Constants;
using System.Text.RegularExpressions;

namespace Forma.Api.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<ApiError> ToApiErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<ApiError>();

            foreach (var entry in modelState)
            {
                var field = entry.Key.ToLower();
                var errorMessages = entry.Value.Errors.Select(e => e.ErrorMessage);

                foreach (var message in errorMessages)
                {
                    var code = GetErrorCode(field, message);
                    errors.Add(new ApiError
                    {
                        Field = field,
                        Code = code
                    });
                }
            }

            return errors;
        }

        private static string GetErrorCode(string field, string message)
        {
            var lowerMsg = message.ToLower();

            // Required field validation
            if (ContainsAny(lowerMsg, "required", "must not be empty"))
                return ErrorCodes.ForField(field, ErrorCodes.Required);

            // Length validations
            if (ContainsAny(lowerMsg, "minimum length", "at least") && ContainsAny(lowerMsg, "character", "length"))
                return ErrorCodes.ForField(field, ErrorCodes.TooShort);

            if (ContainsAny(lowerMsg, "maximum length", "at most", "exceed") && ContainsAny(lowerMsg, "character", "length"))
                return ErrorCodes.ForField(field, ErrorCodes.TooLong);

            // Range validations
            if (Regex.IsMatch(lowerMsg, @"must be between .* and .*"))
                return ErrorCodes.ForField(field, ErrorCodes.OutOfRange);

            if (ContainsAny(lowerMsg, "must be greater than", "must be at least"))
                return ErrorCodes.ForField(field, ErrorCodes.TooSmall);

            if (ContainsAny(lowerMsg, "must be less than", "must be at most"))
                return ErrorCodes.ForField(field, ErrorCodes.TooLarge);

            // Format validations
            if (ContainsAny(lowerMsg, "email", "e-mail") && ContainsAny(lowerMsg, "not valid", "invalid", "format"))
                return ErrorCodes.ForField(field, ErrorCodes.InvalidFormat);

            if (ContainsAny(lowerMsg, "phone", "telephone") && ContainsAny(lowerMsg, "not valid", "invalid", "format"))
                return ErrorCodes.ForField(field, ErrorCodes.InvalidFormat);

            if (ContainsAny(lowerMsg, "url") && ContainsAny(lowerMsg, "not valid", "invalid", "format"))
                return ErrorCodes.ForField(field, ErrorCodes.InvalidFormat);

            // Pattern/Regex validations
            if (ContainsAny(lowerMsg, "match the required pattern", "regular expression", "format is invalid"))
                return ErrorCodes.ForField(field, ErrorCodes.InvalidFormat);

            // Type validations
            if (ContainsAny(lowerMsg, "not a valid", "invalid type", "must be a"))
                return ErrorCodes.ForField(field, ErrorCodes.InvalidType);

            // Comparison validations
            if (ContainsAny(lowerMsg, "must match", "must be equal to", "does not match"))
                return ErrorCodes.ForField(field, ErrorCodes.Mismatch);

            // Credit card
            if (ContainsAny(lowerMsg, "credit card"))
                return ErrorCodes.ForField(field, ErrorCodes.InvalidFormat);

            // Default fallback for any other validation error
            return ErrorCodes.ForField(field, ErrorCodes.Invalid);
        }

        private static bool ContainsAny(string source, params string[] values)
        {
            return values.Any(value => source.Contains(value, StringComparison.OrdinalIgnoreCase));
        }

    }
}
