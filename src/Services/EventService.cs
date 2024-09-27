using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly ITagRepository _tagRepository;
    public EventService(IEventRepository eventRepository, IEventCategoryRepository eventCategoryRepository, IRoomRepository roomRepository, 
        IParticipantRepository participantRepository, ITagRepository tagRepository)
    {
        _eventRepository = eventRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _participantRepository = participantRepository;
        _tagRepository = tagRepository;
    }

    public async Task<EventDto?> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate, 
        List<string>? participantIds, List<string>? tagIds)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null) return null;

        var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);
        if (eventCategory == null) return null;

        List<Participant> participants = [];
        List<ParticipantDto> participantDtos = [];
        
        for (int i = 0; i < participantIds?.Count; i++) {
            var participant = await _participantRepository.GetByIdAsync(participantIds[i]);
            if (participant == null) return null;
            if (!participants.Contains(participant)) {
                participants.Add(participant);
                participantDtos.Add( new ParticipantDto {
                    Id = participant.Id,
                    FirstName = participant.FirstName,
                    LastName = participant.LastName,
                    Email = participant.Email,
                    Description = participant.Description,
                    Address = participant.Address,
                    City = participant.City,
                    Country = participant.Country,
                    DateOfBirth = participant.DateOfBirth
                });
            }
        }

        List<Tag> tags = [];
        List<TagDto> tagDtos = [];

        for (int i = 0; i < tagIds?.Count; i++) {
            var tag = await _tagRepository.GetByIdAsync(tagIds[i]);
            if (tag == null) return null;
            if (!tags.Contains(tag)) {
                tags.Add(tag);
                tagDtos.Add( new TagDto {
                    Id = tag.Id,
                    Icon = tag.Icon,
                    Name = tag.Name,
                });
            }
        }
        
        var eventToAdd = new Event {
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            Room = room,
            RoomId = room.Id,
            EventCategory = eventCategory,
            EventCategoryId = eventCategory.Id,
            Participants = participants,
            Tags = tags
        };
        
        await _eventRepository.CreateAsync(eventToAdd);

        var eventDto = new EventDto {
            Id = eventToAdd.Id,
            Description = description,
            StartDate = eventToAdd.StartDate,
            EndDate = eventToAdd.EndDate,
            Participants = participantDtos,
            Tags = tagDtos,
            Room = new RoomDto {
                Id = eventToAdd.Room.Id,
                Name = eventToAdd.Room.Name
            },
            EventCategory = new EventCategoryDto {
                Id = eventToAdd.EventCategory.Id,
                Name = eventToAdd.EventCategory.Name,
                Icon = eventToAdd.EventCategory.Icon,
            }
        };

        return eventDto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var eventToDelete = await _eventRepository.GetByIdAsync(id);
        if (eventToDelete == null) return false;

        await _eventRepository.DeleteAsync(eventToDelete);

        return true;
    }

    public async Task<EventDto?> GetByIdAsync(string id)
    {
        var eventToGet = await _eventRepository.GetByIdAsync(id);
        if (eventToGet == null) return null;
        
        var eventDto = new EventDto {
            EndDate = eventToGet.EndDate,
            StartDate = eventToGet.StartDate,
            Id = eventToGet.Id,
            Room = new RoomDto {
                Id = eventToGet.Room.Id,
                Name = eventToGet.Room.Name
            },
            EventCategory = new EventCategoryDto {
                Id = eventToGet.EventCategory.Icon,
                Name = eventToGet.EventCategory.Name,
                Icon = eventToGet.EventCategory.Icon,
            },
            Participants = eventToGet.Participants.Select(participant => new ParticipantDto {
                Id = participant.Id,
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                Email = participant.Email,
                Description = participant.Description,
                Address = participant.Address,
                City = participant.City,
                Country = participant.Country,
                DateOfBirth = participant.DateOfBirth
            }).ToList(),
            Tags = eventToGet.Tags.Select(tag => new TagDto {
                Id = tag.Id,
                Icon = tag.Icon,
                Name = tag.Name,
            }).ToList()
        };

        return eventDto;
    }

    public async Task<EventDto?> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription, 
        string? newEventCategoryId, List<string>? newParticipantIds, List<string>? newTagIds)
    {
        var eventToUpdate = await _eventRepository.GetByIdAsync(id);
        if (eventToUpdate == null || (newStartDate == null && newEndDate == null && newRoomId == null && newDescription == null && 
            newEventCategoryId == null && newParticipantIds == null && newTagIds == null)) return null;

        if (newRoomId != null) {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return null;

            eventToUpdate.Room = room;
            eventToUpdate.RoomId = room.Id;
        }

        if (newEventCategoryId != null) {
            var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
            if (eventCategory == null) return null;

            eventToUpdate.EventCategory = eventCategory;
            eventToUpdate.EventCategoryId = eventCategory.Id;
        }

        List<Participant> participants = [];
        for (int i = 0; i < newParticipantIds?.Count; i++) {
            var participantToAdd = eventToUpdate.Participants.Find(x => x.Id == newParticipantIds[i]);
            if (participantToAdd == null) {
                var participant = await _participantRepository.GetByIdAsync(newParticipantIds[i]);
                if (participant == null) return null;
                participants.Add(participant);
            } else {
                participants.Add(participantToAdd);
            }
        }

        List<Tag> tags = [];
        for (int i = 0; i < newTagIds?.Count; i++) {
            var tagToAdd = eventToUpdate.Tags.Find(x => x.Id == newTagIds[i]);
            if (tagToAdd == null) {
                var tag = await _tagRepository.GetByIdAsync(newTagIds[i]);
                if (tag == null) return null;
                tags.Add(tag);
            } else {
                tags.Add(tagToAdd);
            }
        }

        eventToUpdate.StartDate = newStartDate ?? eventToUpdate.StartDate;
        eventToUpdate.EndDate = newEndDate ?? eventToUpdate.EndDate;
        eventToUpdate.Description = newDescription ?? eventToUpdate.Description;
        eventToUpdate.Participants = participants.Count > 0 ? participants : eventToUpdate.Participants;
        eventToUpdate.Tags = tags.Count > 0 ? tags : eventToUpdate.Tags;

        await _eventRepository.UpdateAsync(eventToUpdate);
        
        var eventDto = new EventDto {
            Id = eventToUpdate.Id,
            Description = eventToUpdate.Description,
            StartDate = eventToUpdate.StartDate,
            EndDate = eventToUpdate.EndDate,
            Participants = eventToUpdate.Participants.Select(p => new ParticipantDto {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
            }).ToList(),
            Tags = eventToUpdate.Tags.Select(t => new TagDto {
                Id = t.Id,
                Name = t.Name,
                Icon = t.Icon,
            }).ToList(),
            Room = new RoomDto {
                Id = eventToUpdate.Room.Id,
                Name = eventToUpdate.Room.Name
            },
            EventCategory = new EventCategoryDto {
                Id = eventToUpdate.EventCategory.Id,
                Name = eventToUpdate.EventCategory.Name,
                Icon = eventToUpdate.EventCategory.Icon,
            }
        };

        return eventDto;
    }
}
