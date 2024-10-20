using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Databases;

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
    public DbSet<Workspace> Workspace { get; set; }
    public DbSet<EventUser> EventUser { get; set; }
    public DbSet<WorkspaceRole> WorkspaceRole { get; set; }
    public DbSet<Invitation> Invitatition { get; set; }
    public DbSet<Notification> Notification { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // var JsonSerializerOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        // var dayOfWeekConverter = new ValueConverter<List<DayOfWeek>?, string?>(
        //     v => v == null ? null : JsonSerializer.Serialize(v, JsonSerializerOptions), // Si `v` est null, retourner null
        //     v => v == null ? null : JsonSerializer.Deserialize<List<DayOfWeek>>(v, JsonSerializerOptions) // Si la chaîne est null, retourner null
        // );

        // // WorkspaceUser
        // builder.Entity<WorkspaceUser>()
        //     .HasKey(uw => new { uw.WorkspaceId, uw.UserId }); 

        // builder.Entity<WorkspaceUser>()
        //     .HasOne(uw => uw.User)
        //     .WithMany(u => u.Workspaces)
        //     .HasForeignKey(uw => uw.UserId);

        // builder.Entity<WorkspaceUser>()
        //     .HasOne(uw => uw.Workspace)
        //     .WithMany(u => u.Users)
        //     .HasForeignKey(uw => uw.WorkspaceId);

        // EventUser
        builder.Entity<EventUser>()
            .HasKey(uw => new { uw.EventId, uw.UserId }); 

        builder.Entity<EventUser>()
            .HasOne(uw => uw.User)
            .WithMany(u => u.Events)
            .HasForeignKey(uw => uw.UserId);

        builder.Entity<EventUser>()
            .HasOne(uw => uw.Event)
            .WithMany(u => u.Users)
            .HasForeignKey(uw => uw.EventId);
    }
}
