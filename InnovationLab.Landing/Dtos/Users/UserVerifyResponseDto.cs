namespace InnovationLabBackend.Api.Dtos.Users
{
    public class UserVerifyResponseDto
    {
        public required string Message { get; set; }
        public required string Token { get; set; }
    }
}