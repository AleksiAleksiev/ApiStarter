namespace NewInterlex.Api.Models.Request
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}