using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InnovationLab.Shared.Enums;
using InnovationLab.Shared.Interfaces;
using Microsoft.AspNetCore.Http;

namespace InnovationLab.Shared.Services;

public class MediaService(ICloudinary cloudinary) : IMediaService
{
    private readonly ICloudinary _cloudinary = cloudinary;

    public async Task<string?> UploadAsync(IFormFile file, MediaType mediaType, string? folder = null, Transformation? transformation = null)
    {
        RawUploadParams? uploadParams = mediaType switch
        {
            MediaType.Image => new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder,
                Transformation = transformation ?? new Transformation().Quality("auto").FetchFormat("auto")
            },
            MediaType.Video => new VideoUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder,
                Transformation = transformation
            },
            MediaType.Pdf => new RawUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder
            },
            _ => null
        };

        if (uploadParams is null)
        {
            return null;
        }

        UploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult.SecureUrl.ToString();
    }
}