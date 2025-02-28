using System;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly MySqlDbContext _context;
    public InvitationRepository(MySqlDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            var res = await _context.Invitatition
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

    public async Task<Invitation?> GetByIdAsync(string id)
    {
        try
        {
            return await _context.Invitatition
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }
    }

    public async Task<List<Invitation>> GetByReceiverUserIdAsync(string userId)
    {
        try
        {
            return await _context.Invitatition
                .Where(x => x.ReceiverId == userId && x.DeletedAt == null)
                .Include(x => x.Sender)
                .ToListAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting invitations by userid '{userId}' : {ex.Message}");
            throw new DbException(DbAction.Read, "invitations", userId);
        }
    }

    public async Task<Invitation> UpdateAsync(Invitation invitation)
    {
        try
        {
            invitation.UpdatedAt = DateTime.UtcNow;
            var res = await _context.SaveChangesAsync();
            return res > 0 ? invitation : throw new DbException(DbAction.Update, "invitation", invitation.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating invitation with Id '{invitation.Id}' : {ex.Message}");
            throw new DbException(DbAction.Update, "invitation", invitation.Id);
        }
    }

    public async Task<Invitation> CreateAsync(Invitation invitation)
    {
        try
        {
            _context.Invitatition.Add(invitation);
            var res = await _context.SaveChangesAsync();
            return res > 0 ? invitation : throw new DbException(DbAction.Create, "invitation");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating invitation : {ex.Message}");
            throw new DbException(DbAction.Create, "invitation");
        }
    }

    public async Task<Invitation?> GetByWorkspaceIdAndReceiverId(string workspaceId, string receiverId)
    {
        try
        {
            return await _context.Invitatition
                .Where(i => i.DeletedAt == null && i.WorkspaceId == workspaceId && i.ReceiverId == receiverId)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting invitation by workspace id and receiver id, {ex.Message}");
            throw new DbException(DbAction.Read, "invitation", "");
        }
    }

    public async Task<Invitation?> GetByIdJoinWorkspaceAndReceiver(string id)
    {
        try
        {
            return await _context.Invitatition
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Include(x => x.Receiver)
                .Include(x => x.Workspace)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }
    }

    public async Task<Invitation?> GetByIdJoinSenderAndReceiver(string id)
    {
        try
        {
            return await _context.Invitatition
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }
    }

    public async Task<List<Invitation>> GetBySenderUserIdAsync(string userId)
    {
        try
        {
            return await _context.Invitatition
                .Where(x => x.SenderId == userId && x.InvitationType == InvitationType.Contact && x.DeletedAt == null)
                .Include(x => x.Receiver)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting invitations by userid '{userId}' : {ex.Message}");
            throw new DbException(DbAction.Read, "invitations", userId);
        }
    }

    public async Task<List<Invitation>> GetByWorkspaceIdJoinWorkspaceMembersAsync(string workspaceId)
    {
        try
        {
            return await _context.Invitatition
                .Where(x => x.WorkspaceId == workspaceId && x.DeletedAt == null)
                .Include(x => x.Workspace)
                    .ThenInclude(w => w!.Users)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting invitations by workspaceId '{workspaceId}' : {ex.Message}");
            throw new DbException(DbAction.Read, "invitations", workspaceId);
        }
    }
}
