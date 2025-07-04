using System;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Utils;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User
        CreateMap<User, UserDto>();
        CreateMap<User, MemberDto>();
        CreateMap<User, UserJoinWorkspaceDto>();
        // ParticipantCategory
        CreateMap<ParticipantCategory, ParticipantCategoryDto>();
        // Participant
        CreateMap<Participant, ParticipantDto>();
        CreateMap<Participant, ParticipantJoinCategoryDto>()
            .ForMember(dest => dest.ParticipantCategory, opt => opt.MapFrom(src => src.ParticipantCategory));
        // Event
        CreateMap<Event, EventDto>();
        CreateMap<EventUser, EventUserDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id));
        // Area
        CreateMap<Area, AreaDto>();
        // Room
        CreateMap<Room, RoomDto>();
        // Invitation
        CreateMap<Invitation, InvitationDto>();
        // Location
        CreateMap<Location, LocationDto>();
        // slot
        CreateMap<Slot, SlotDto>();
        // EventCategory
        CreateMap<EventCategory, EventCategoryDto>();

        // WorkspaceRole
        CreateMap<WorkspaceRole, WorkspaceRoleDto>();
        // Tag
        CreateMap<Tag, TagDto>();
        // Workspace
        CreateMap<Workspace, WorkspaceDto>();
        CreateMap<Workspace, WorkspaceDetailsDto>()
            .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations))
            .ForMember(dest => dest.Slots, opt => opt.MapFrom(src => src.Slots))
            .ForMember(dest => dest.EventCategories, opt => opt.MapFrom(src => src.EventCategories))
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
            .ForMember(dest => dest.ParticipantCategories, opt => opt.MapFrom(src => src.ParticipantCategories))
            .ForMember(dest => dest.WorkspaceRoles, opt => opt.MapFrom(src => src.WorkspaceRoles))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users));
    }
}
