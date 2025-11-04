using Microsoft.AspNetCore.Mvc.ModelBinding;
using Forma.Api.Responses;

namespace Forma.Api.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<ApiError> ToApiErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<ApiError>();

            foreach (var entry in modelState)
            {
                var field = entry.Key;
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

            if (lowerMsg.Contains("required"))
                return $"error.{field.ToLower()}.required";
            if (lowerMsg.Contains("minimum") || lowerMsg.Contains("length") || lowerMsg.Contains("at least"))
                return $"error.{field.ToLower()}.too_short";
            if (lowerMsg.Contains("email"))
                return $"error.{field.ToLower()}.invalid_email_format";
            if (lowerMsg.Contains("email_already_exists"))
                return $"error.{field.ToLower()}.email_already_exists";
            if (lowerMsg.Contains("invalid_credentials"))
                return $"error.{field.ToLower()}.invalid_credentials";

            // Default error code
            return $"error.{field.ToLower()}.invalid";
        }
    }
}
