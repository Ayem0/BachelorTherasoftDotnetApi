using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IEventCategoryRepository : IBaseRepository<EventCategory>
{
    Task<List<EventCategory>> GetByWorkpaceIdAsync(string id);
}
