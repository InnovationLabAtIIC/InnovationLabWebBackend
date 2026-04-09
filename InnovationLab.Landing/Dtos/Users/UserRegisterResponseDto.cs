namespace InnovationLabBackend.Api.Dtos.Users
{
    public class UserRegisterResponseDto
    {
        public required string Message { get; set; }
        public required string QrCodeUrl { get; set; }
    }
}