using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.EventRegistrations
{
    [AutoMap(typeof(EventRegistration), ReverseMap = true)]
    public class EventRegistrationDto : CreateTeamMemberDto
    {
        public List<CreateTeamMemberDto>? TeamMembers { get; set; }
    }
}