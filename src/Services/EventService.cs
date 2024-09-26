using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventService : IEventService
{
    public Task<EventDto?> CreateAsync(string name, string roomId, string eventCategoryId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto?> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(string id, string newName)
    {
        throw new NotImplementedException();
    }
}
