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

public class InvitationRepository : BaseRepository<Invitation>, IInvitationRepository
{
    public InvitationRepository(MySqlDbContext context, ILogger<Invitation> logger) : base(context, logger)
    {
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
