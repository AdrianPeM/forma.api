namespace Forma.Api.Responses
{
    public class SignupResponse
    {
        public required Guid Id { get; set; }
    }

    public class LoginResponse
    {
        public required string Email { get; set; }
        public required string AccessToken { get; set; }
        public required int ExpiresIn { get; set; }
    }
}
