namespace InnovationLabBackend.Api.Dtos.Users
{
    public class UserLoginResponseDto
    {
        public required string Message { get; set; }
        public bool Requires2fa { get; set; } = true;
        public string? Email { get; set; }
    }
}