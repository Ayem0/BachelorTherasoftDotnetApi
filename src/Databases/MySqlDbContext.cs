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
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Workspace>().HasQueryFilter(w => w.DeletedAt == null);
        builder.Entity<Area>().HasQueryFilter(a => a.DeletedAt == null);
        builder.Entity<Location>().HasQueryFilter(l => l.DeletedAt == null);
        builder.Entity<Room>().HasQueryFilter(r => r.DeletedAt == null);
        builder.Entity<Event>().HasQueryFilter(e => e.DeletedAt == null);
        builder.Entity<Participant>().HasQueryFilter(p => p.DeletedAt == null);
        builder.Entity<Tag>().HasQueryFilter(t => t.DeletedAt == null);
        builder.Entity<Document>().HasQueryFilter(d => d.DeletedAt == null);
        builder.Entity<DocumentCategory>().HasQueryFilter(dc => dc.DeletedAt == null);
        builder.Entity<ParticipantCategory>().HasQueryFilter(pc => pc.DeletedAt == null);
        builder.Entity<EventCategory>().HasQueryFilter(ec => ec.DeletedAt == null);
        builder.Entity<Slot>().HasQueryFilter(s => s.DeletedAt == null);
        builder.Entity<WorkspaceRole>().HasQueryFilter(wr => wr.DeletedAt == null);
        builder.Entity<Invitation>().HasQueryFilter(i => i.DeletedAt == null);

        builder.Entity<Notification>().HasQueryFilter(n => n.DeletedAt == null);
        // Contacts
        builder.Entity<User>()
            .HasMany(u => u.Contacts)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
            "UserContactUser",
            u => u.HasOne<User>().WithMany().HasForeignKey("ContactId"),
            u => u.HasOne<User>().WithMany().HasForeignKey("UserId")
            );

        builder.Entity<User>()
            .HasMany(u => u.BlockedUsers)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
            "UserBlockedUser",
            u => u.HasOne<User>().WithMany().HasForeignKey("BlockedUserId"),
            u => u.HasOne<User>().WithMany().HasForeignKey("UserId")
            ); ;

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

        // builder.Entity<Area>().HasKey(x => x.Id);
        // builder.Entity<Workspace>().HasKey(x => x.Id);
        // builder.Entity<Location>().HasKey(x => x.Id);
        // builder.Entity<Room>().HasKey(x => x.Id);
        // builder.Entity<Event>().HasKey(x => x.Id);
        // builder.Entity<Participant>().HasKey(x => x.Id);
        // builder.Entity<Tag>().HasKey(x => x.Id);
        // builder.Entity<Document>().HasKey(x => x.Id);
        // builder.Entity<DocumentCategory>().HasKey(x => x.Id);
        // builder.Entity<ParticipantCategory>().HasKey(x => x.Id);
        // builder.Entity<EventCategory>().HasKey(x => x.Id);
        // builder.Entity<Slot>().HasKey(x => x.Id);
        // builder.Entity<WorkspaceRole>().HasKey(x => x.Id);
        // builder.Entity<Invitation>().HasKey(x => x.Id);
        // builder.Entity<Notification>().HasKey(x => x.Id);

        // var JsonSerializerOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        // var dayOfWeekConverter = new ValueConverter<List<DayOfWeek>?, string?>(
        //     v => v == null ? null : JsonSerializer.Serialize(v, JsonSerializerOptions), // Si `v` est null, retourner null
        //     v => v == null ? null : JsonSerializer.Deserialize<List<DayOfWeek>>(v, JsonSerializerOptions) // Si la chaîne est null, retourner null
        // );

        // Configuration pour DateOnly
        // builder.Entity<Slot>()
        //     .Property(s => s.StartDate)
        //     .HasConversion(
        //         d => d.ToDateTime(TimeOnly.MinValue), // Conversion de DateOnly vers DateTime
        //         d => DateOnly.FromDateTime(d)        // Conversion de DateTime vers DateOnly
        //     );

        // builder.Entity<Slot>()
        //     .Property(s => s.EndDate)
        //     .HasConversion(
        //         d => d.ToDateTime(TimeOnly.MinValue),
        //         d => DateOnly.FromDateTime(d)
        //     );

        // // // Configuration pour TimeOnly
        // builder.Entity<Slot>()
        //     .Property(s => s.StartTime)
        //     .HasConversion(
        //         t => t.ToTimeSpan(),          // Conversion de TimeOnly vers TimeSpan
        //         t => TimeOnly.FromTimeSpan(t)       // Conversion de TimeSpan vers TimeOnly
        //     );

        // builder.Entity<Slot>()
        //     .Property(s => s.EndTime)
        //     .HasConversion(
        //         t => t.ToTimeSpan(),
        //         t => TimeOnly.FromTimeSpan(t)
        //     );
    }
}
