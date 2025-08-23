using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Auth.Dtos;

public record EmailRequestDto([EmailAddress] string Email);