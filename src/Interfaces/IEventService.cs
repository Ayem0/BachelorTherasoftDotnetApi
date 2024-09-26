using System;
using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IEventService
{
    Task<EventDto?> GetByIdAsync(string id);
    Task<EventDto?> CreateAsync(string name, string roomId, string eventCategoryId);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, string newName);
}
