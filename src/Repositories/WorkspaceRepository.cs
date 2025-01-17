using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly MySqlDbContext _context;
    public WorkspaceRepository(MySqlDbContext context) 
    {
        _context = context;
    }

    public async Task<Workspace> CreateAsync(Workspace workspace)
    {
        try
        {    
            workspace.CreatedAt = DateTime.UtcNow;
            _context.Workspace.Add(workspace);
            var res = await _context.SaveChangesAsync();
            return workspace ?? throw new DbException(DbAction.Create, "Workspace");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating workspace : {ex.Message}");
            throw new DbException(DbAction.Create, "Workspace");
        }   
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {    
            var res = await _context.Workspace
                .Where(x => x.Id == id && x.DeletedAt == null)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.DeletedAt, DateTime.UtcNow));
            
            return res > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting workspace with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Delete, "Workspace", id);
        }   
    }

    public async Task<Workspace?> GetByIdAsync(string id)
    {
        try
        {    
            return await _context.Workspace
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }   
    }

    public async Task<Workspace[]> GetByUserIdAsync(string id)
    {
        try
        {    
            return await _context.Workspace
                .Where(x => x.Users.Any(x => x.Id == id && x.DeletedAt == null) && x.DeletedAt == null)
                .ToArrayAsync();
                
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace for user with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }   
    }

    public async Task<Workspace?> GetDetailsByIdAsync(string id)
    {
        try
        {    
            return await _context.Workspace
                .Include(x => x.Users.Where(y => y.DeletedAt == null))
                .Include(x => x.Locations.Where(y => y.DeletedAt == null))
                    .ThenInclude(x => x.Areas.Where(y => y.DeletedAt == null))
                        .ThenInclude(x => x.Rooms.Where(y => y.DeletedAt == null))
                .Include(x => x.WorkspaceRoles.Where(y => y.DeletedAt == null))
                .Include(x => x.Tags.Where(y => y.DeletedAt == null))
                .Include(x => x.EventCategories.Where(y => y.DeletedAt == null))
                .Include(x => x.Slots.Where(y => y.DeletedAt == null))
                .Include(x => x.Participants.Where(y => y.DeletedAt == null))
                .Include(x => x.ParticipantCategories.Where(y => y.DeletedAt == null))
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }   
    }

    public async Task<Workspace?> GetJoinUsersByIdAsync(string id)
    {
        try
        {    
            return await _context.Workspace
                .Include(x => x.Users.Where(y => y.DeletedAt == null))
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }   
    }

    public async Task<Workspace> UpdateAsync(Workspace workspace)
    {
        try
        {    
            workspace.UpdatedAt = DateTime.UtcNow;
            var res = await _context.SaveChangesAsync();
            return res > 0 ? workspace : throw new DbException(DbAction.Update, "Workspace", workspace.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating workspace with Id '{workspace.Id}' : {ex.Message}");
            throw new DbException(DbAction.Update, "Workspace", workspace.Id);
        }   
    }
}
