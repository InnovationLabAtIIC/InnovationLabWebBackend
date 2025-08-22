namespace InnovationLab.Auth.Dtos;

public record TokenResponseDto(
    string Token,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt
);