using CloudinaryDotNet;
using InnovationLab.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace InnovationLab.Shared.Interfaces;

public interface IMediaService
{
    Task<string?> UploadAsync(IFormFile file, MediaType mediaType, string? folder = null, Transformation? transformation = null);
}