namespace Hauto.Models.DTOs
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AddPasscodeDto
    {
        public int UserId { get; set; }
        public string Passcode { get; set; }
    }
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class GetUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Passcode { get; set; }
    }
    public class UserResponse : BaseResponse
    {
        public GetUserDto Data { get; set; } = new GetUserDto();
    }
}
