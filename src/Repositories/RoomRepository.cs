using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly MySqlDbContext _context;
    public RoomRepository(MySqlDbContext context) 
    {
        _context = context;
    }

    public async Task<Room> CreateAsync(Room Room)
    {
        try
        {    
            Room.CreatedAt = DateTime.UtcNow;
            _context.Room.Add(Room);
            var res = await _context.SaveChangesAsync();
            return Room ?? throw new DbException(DbAction.Create, "Room");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Room : {ex.Message}");
            throw new DbException(DbAction.Create, "Room");
        }   
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {    
            var res = await _context.Room
                .Where(x => x.Id == id && x.DeletedAt == null)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.DeletedAt, DateTime.UtcNow));
            
            return res > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Delete, "Room", id);
        }   
    }

    public async Task<Room?> GetByIdAsync(string id)
    {
        try
        {    
            return await _context.Room
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }   
    }

    public async Task<Room?> GetDetailsByIdAsync(string id)
    {
        try
        {    
            return await _context.Room
                .Include(x => x.Events.Where(y => y.DeletedAt == null))
                .Include(x => x.Slots.Where(y => y.DeletedAt == null))                
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }   
    }

    public async Task<Room> UpdateAsync(Room Room)
    {
        try
        {    
            Room.UpdatedAt = DateTime.UtcNow;
            var res = await _context.SaveChangesAsync();
            return res > 0 ? Room : throw new DbException(DbAction.Update, "Room", Room.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating Room with Id '{Room.Id}' : {ex.Message}");
            throw new DbException(DbAction.Update, "Room", Room.Id);
        }   
    }
}
