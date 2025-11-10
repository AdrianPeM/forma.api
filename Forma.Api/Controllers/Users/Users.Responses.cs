using Forma.Api.Models;

namespace Forma.Api.Responses
{
    public class GetUsersResponse
    {
        public required List<User> Users { get; set; }
    }

    public class GetUserResponse
    {
        public required User User { get; set; }
    }

    public class PutUserResponse
    {
        public required User UpdatedUser { get; set; }
    }

    public class DeleteUserResponse
    {
        public required bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }  // Optional
    }


}