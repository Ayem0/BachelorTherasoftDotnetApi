using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IConversationService
{
    Task<ConversationDto?> GetByIdAsync(string id);
    Task<ConversationDto?> CreateAsync(List<string> usersId, string? name);
    Task<bool> DeleteAsync(string id);
    Task<ConversationDto?> UpdateAsync(string id, string? newName, List<string>? newUsers);
    Task<List<ConversationDto>?> GetByUserIdAsync(string userId);
}
