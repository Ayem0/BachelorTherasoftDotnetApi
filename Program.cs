
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Hubs;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Repositories;
using BachelorTherasoftDotnetApi.src.Services;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http,
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearer"
                }
            },
            new string[] {}
        }
    });
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Bachelor Therasoft",
        Description = "An ASP.NET Core Web API for collaborative agenda"
    });
    // Ajoute les commentaire du code dans le swagger 
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
// MySQL service
builder.Services.AddDbContext<MySqlDbContext>( 
    options => options.UseMySQL(builder.Configuration.GetConnectionString("MySQL")!)
);
// Identity service
builder.Services.AddIdentityApiEndpoints<User>(options => {
    // options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
    // options.User.AllowedUserNameCharacters = "azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN0123456789";
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 10;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
}).AddEntityFrameworkStores<MySqlDbContext>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "SampleInstance";
});
// Email service
//builder.Services.AddTransient<IEmailSender, EmailSender>();
//builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// Cors client
builder.Services.AddCors(options => options.AddPolicy("Client", 
    policy => policy.WithOrigins("http://localhost:4200", "http://localhost:4000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
));
// Cors server ssr
builder.Services.AddCors(options => options.AddPolicy("Server", 
    policy => policy.WithOrigins("http://client:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
));
// Authentication service
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// SignalR
builder.Services.AddSignalR();

// Custom Repositories
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IWorkspaceRoleRepository, WorkspaceRoleRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IParticipantCategoryRepository, ParticipantCategoryRepository>();
// builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
// builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ISlotRepository, SlotRepository>();
// builder.Services.AddScoped<IEventMemberRepository, EventMemberRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


// Custom Services
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IWorkspaceRoleService, WorkspaceRoleService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventCategoryService, EventCategoryService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IParticipantCategoryService, ParticipantCategoryService>();
// builder.Services.AddScoped<IConversationService, ConversationService>();
// builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ISlotService, SlotService>();
// builder.Services.AddScoped<IEventMemberService, EventMemberService>();


// Utils Services
// builder.Services.AddScoped<IRepetitionService, RepetitionService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Client");
app.UseCors("Server");
app.UseHttpsRedirection();
app.UseExceptionHandler("/Api/Error");
app.UseAuthentication();

app.UseAuthorization();

app.MapIdentityApi<User>();
// TODO a delete lors de l'auth controller et auth service
app.MapPost("/registerBis", async (
    RegisterBisRequest request,
    UserManager<User> userManager) =>
{
    var user = new User()
    {
        UserName = request.Email,
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName
    };

    var result = await userManager.CreateAsync(user, request.Password);

    if (result.Succeeded)
    {
        return Results.Ok();
    }

    return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
});
app.MapControllers();
app.MapHub<GlobalHub>("/global");
app.MapHub<WorkspaceHub>("/workspace");

app.Run();
