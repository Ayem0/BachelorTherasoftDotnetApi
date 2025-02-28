using System;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Utils;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Configure your mappings here
        CreateMap<Workspace, WorkspaceDto>();

        // Mapping Workspace to WorkspaceDetailsDto
        CreateMap<Workspace, WorkspaceDetailsDto>()
            .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations))
            .ForMember(dest => dest.Slots, opt => opt.MapFrom(src => src.Slots))
            .ForMember(dest => dest.EventCategories, opt => opt.MapFrom(src => src.EventCategories))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
            .ForMember(dest => dest.ParticipantCategories, opt => opt.MapFrom(src => src.ParticipantCategories))
            .ForMember(dest => dest.WorkspaceRoles, opt => opt.MapFrom(src => src.WorkspaceRoles))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users));
        // .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(u => u.User)));

        // CreateMap<WorkspaceUser, UserDto>()
        //     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
        //     .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
        //     .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName));

        CreateMap<Location, LocationDto>();
        CreateMap<Slot, SlotDto>();
        CreateMap<EventCategory, EventCategoryDto>();
        CreateMap<Participant, ParticipantDto>();
        CreateMap<ParticipantCategory, ParticipantCategoryDto>();
        CreateMap<WorkspaceRole, WorkspaceRoleDto>();
        CreateMap<Tag, TagDto>();
        CreateMap<User, UserDto>();
        CreateMap<Area, AreaDto>();
        CreateMap<Room, RoomDto>();
        CreateMap<Invitation, InvitationDto>();
        CreateMap<Event, EventDto>()
        .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(x => x.User)));
    }
}
