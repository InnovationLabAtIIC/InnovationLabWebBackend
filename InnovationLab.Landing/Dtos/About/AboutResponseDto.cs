﻿using Mapster;

namespace InnovationLab.Landing.Dtos.About;

[AdaptFrom(typeof(Models.About))]
public record AboutResponseDto
(
    Guid Id,
    string? Mission,
    string? Vision,
    string? ParentOrgName,
    string? ParentOrgDescription,
    string? ParentOrgLogoUrl,
    string? ParentOrgWebsiteUrl,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? DeletedAt
);
