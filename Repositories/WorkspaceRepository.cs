using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Databases;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly MySqlDbContext _context;
    
    public WorkspaceRepository(MySqlDbContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(string workspaceId, User user)
    {
        var workspace = await GetWorkspaceAsync(workspaceId);

        if ( workspace == null || workspace.Users.Find(x => x.Id == user.Id) != null) return;

        workspace.Users.Add(user);

        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(string name, User user)
    {
        var workspace = new Workspace{
            Name = name,
            Users = [user]
        };

        await _context.Workspace.AddAsync(workspace);
    }

    public async Task DeleteAsync(string workspaceId)
    {
        var workspace = await GetWorkspaceAsync(workspaceId);

        if ( workspace == null) return;

        workspace.DeletedAt = DateTime.Now;

        _context.Workspace.Update(workspace);

        await _context.SaveChangesAsync();
    }

    public async Task<Workspace?> GetWorkspaceAsync(string workspaceId)
    {
        var workspace = await _context.Workspace.Where(w => w.Id == workspaceId && w.DeletedAt == null).FirstAsync();

        return workspace;
    }

    public async Task<List<Workspace>?> GetWorkspacesAsync(string[] workspaceIds)
    {
        var workspaces = await _context.Workspace.Where(w => workspaceIds.Contains(w.Id) && w.DeletedAt != null).ToListAsync();

        return workspaces;
    }

    public async Task RemoveUserAsync(string workspaceId, User user)
    {
        var workspace = await GetWorkspaceAsync(workspaceId);

        if (workspace == null || workspace.Users.Find(x => x == user) == null) return;

        workspace.Users.Remove(user);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(string workspaceId, string name)
    {
        var workspace = await GetWorkspaceAsync(workspaceId);

        if (workspace == null) return;

        workspace.Name = name;

        _context.Workspace.Update(workspace);

        await _context.SaveChangesAsync();
    }
}
