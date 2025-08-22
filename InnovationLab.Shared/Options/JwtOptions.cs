namespace InnovationLab.Shared.Options;

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public uint ExpiryMinutes { get; set; } = 30;
}