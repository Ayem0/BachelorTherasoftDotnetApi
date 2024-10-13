using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface ITagRepository : IBaseRepository<Tag>
{
    // new Task<Tag?> GetByIdAsync(string id);
    Task<Tag?> GetByIdJoinWorkspaceAsync(string id);

}
