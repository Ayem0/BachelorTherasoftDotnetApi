using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class WorkspaceRepository : BaseMySqlRepository<Workspace>, IWorkspaceRepository
{
    public WorkspaceRepository(MySqlDbContext context) : base(context)
    {
    }

    // public async Task<WorkspaceDetailsDto?> GetDetailsByIdAsync(string id)
    // {
    //     Workspace? workspace = await _context.Workspace
    //         .Include(wu => wu.Users)
    //             .ThenInclude(wu => wu.User)
    //         .Include(w => w.WorkspaceRoles)
    //         .Include(w => w.Participants)
    //         .Include(w => w.ParticipantCategories)
    //         .Include(w => w.EventCategories)
    //         .Include(w => w.Locations)
    //         .Include(w => w.Slots)
    //         .Include(w => w.Tags)
    //         .Where(w => w.Id == id && w.DeletedAt == null && w.Users.All(u => u.DeletedAt == null && u.User.DeletedAt == null) && w.WorkspaceRoles.All(wr => wr.DeletedAt == null) 
    //             && w.Participants.All(wr => wr.DeletedAt == null) && w.ParticipantCategories.All(wr => wr.DeletedAt == null) && w.EventCategories.All(wr => wr.DeletedAt == null) 
    //             && w.Locations.All(wr => wr.DeletedAt == null) && w.Slots.All(wr => wr.DeletedAt == null) && w.Tags.All(wr => wr.DeletedAt == null))
    //         .FirstOrDefaultAsync();
    //     return workspace != null ? _mapper.Map<WorkspaceDetailsDto>(workspace) : null;
    // }
}
