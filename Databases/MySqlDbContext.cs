using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BachelorTherasoftDotnetApi.Databases;

public class MySqlDbContext : IdentityDbContext<User, Role, string>
{
     public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
    {
    }
    public DbSet<Area> Area { get; set; }
    public DbSet<Document> Document { get; set; }
    public DbSet<DocumentCategory> DocumentCategory { get; set; }
    public DbSet<Event> Event { get; set; }
    public DbSet<EventCategory> EventCategory { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<Participant> Participant { get; set; }
    public DbSet<ParticipantCategory> ParticipantCategory { get; set; }
    public DbSet<Room> Room { get; set; }
    public DbSet<Slot> Slot { get; set; }
    //public DbSet<User_Workspace> User_Workspace { get; set; }
    public DbSet<Workspace> Workspace { get; set; }
    public DbSet<WorkspaceRight> WorkspaceRight { get; set; }
    public DbSet<WorkspaceRole> WorkspaceRole { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var JsonSerializerOptions = new JsonSerializerOptions  { Converters = { new JsonStringEnumConverter() } };
        var dayOfWeekConverter = new ValueConverter<List<DayOfWeek>?, string?>(
            v => v == null ? null : JsonSerializer.Serialize(v, JsonSerializerOptions), // Si `v` est null, retourner null
            v => v == null ? null : JsonSerializer.Deserialize<List<DayOfWeek>>(v, JsonSerializerOptions) // Si la chaîne est null, retourner null
        );

       //builder.Entity<User_Workspace>()
       //     .HasKey(uw => new { uw.UserId, uw.WorkspaceId }); 

       // builder.Entity<User_Workspace>()
       //     .HasOne(uw => uw.User)
       //     .WithMany(u => u.Workspaces)
       //     .HasForeignKey(uw => uw.UserId);

       // builder.Entity<User_Workspace>()
       //     .HasOne(uw => uw.Workspace)
       //     .WithMany(w => w.Users)
       //     .HasForeignKey(uw => uw.WorkspaceId);

        builder.Entity<Slot>()
            .Property(e => e.Days)
            .HasConversion(dayOfWeekConverter); 
    }
    
}
