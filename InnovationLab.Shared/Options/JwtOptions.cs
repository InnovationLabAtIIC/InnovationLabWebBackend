namespace InnovationLab.Shared.Options;

public sealed class JwtOptions
{
    public string Secret { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public uint ExpiryMinutes { get; init; } = 30;
}