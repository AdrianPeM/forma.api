namespace Forma.Api.Responses
{
    public class ApiError
    {
        public string Field { get; set; } = default!;
        public string Code { get; set; } = default!;
    }

    public class ApiErrorResponse
    {
        public List<ApiError> Errors { get; set; } = new();
    }
}
