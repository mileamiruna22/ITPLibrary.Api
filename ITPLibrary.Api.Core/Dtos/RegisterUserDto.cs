namespace ITPLibrary.Api.Core.Dtos
{
    public class RegisterUserDto
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}