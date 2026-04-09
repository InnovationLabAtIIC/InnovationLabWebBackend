﻿using InnovationLab.Shared.Models;

namespace InnovationLab.Landing.Models;

public class About : BaseModel
{
    public string? Mission { get; set; }
    public string? Vision { get; set; }
    public string? ParentOrgName { get; set; }
    public string? ParentOrgDescription { get; set; }
    public string? ParentOrgLogoUrl { get; set; }
    public string? ParentOrgWebsiteUrl { get; set; }
}
