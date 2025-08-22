using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Auth.Dtos;

public record TokenRequestDto([Required] string RefreshToken);