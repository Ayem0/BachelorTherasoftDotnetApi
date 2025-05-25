
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Hubs;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Repositories;
using BachelorTherasoftDotnetApi.src.Services;
using BachelorTherasoftDotnetApi.src.Utils;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;
using System.Threading.RateLimiting;
// using MySqlConnector;


var builder = WebApplication.CreateBuilder(args);
// Controllers
builder.Services.AddControllers();

// MySQL service
builder.Services.AddDbContext<MySqlDbContext>(
    (sp, options) => options.UseMySQL(builder.Configuration.GetConnectionString("MySQL")!)
);

// builder.Services.AddDbContext<MySqlDbContext>(
//     options => options.UseMySql(
//         new MySqlConnection(builder.Configuration.GetConnectionString("MySQL")),
//         new MySqlServerVersion(new Version(8, 0, 38)),
//         options => options.EnableRetryOnFailure()
//     ));

// Redis service
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!)
);
builder.Services.AddSingleton<IRedisService, RedisService>();



// Email service
//builder.Services.AddTransient<IEmailSender, EmailSender>();
// builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// Cors client
builder.Services.AddCors(options => options.AddPolicy("Client",
    policy => policy.WithOrigins("http://localhost:4200", "http://localhost:4444")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
));



// Authentication service
var authSection = builder.Configuration.GetSection("Auth");
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddGoogleOpenIdConnect(options =>
    {
        var googleSection = authSection.GetSection("Google");
        options.ClientId = googleSection["ClientId"]!;
        options.ClientSecret = googleSection["ClientSecret"]!;
        options.CallbackPath = "/signin-oidc";
        options.SignInScheme = IdentityConstants.ExternalScheme;
        // options.NonceCookie.SameSite = SameSiteMode.None;
        // options.NonceCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ProtocolValidator.RequireNonce = false;
        // options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
        // options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        // options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstName");
        // options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastName");
        // options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
    });
builder.Services.AddAuthorization();

// Identity service
builder.Services.AddIdentityApiEndpoints<User>(options =>
{
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
// Rate Limiting middleware
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("CompositePolicy", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var user = context.User?.Identity?.IsAuthenticated == true
                    ? context.User.Identity.Name
                    : "anonymous";
        var endpoint = context.Request.Path.ToString().ToLowerInvariant();
        var method = context.Request.Method;
        var key = $"{ip}:{user}:{method}:{endpoint}";
        return RateLimitPartition.GetTokenBucketLimiter(
            key,
            key => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 50,
                TokensPerPeriod = 50,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }
        );
    });

    options.AddPolicy("AuthPolicy", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var user = context.User?.Identity?.Name ?? ip;
        var endpoint = context.Request.Path.ToString().ToLowerInvariant();
        var method = context.Request.Method;
        var key = $"{ip}:{user}:{method}:{endpoint}";
        return RateLimitPartition.GetTokenBucketLimiter(
            key,
            _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 5,
                TokensPerPeriod = 1,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            });
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// SignalR
builder.Services.AddSignalR();
builder.Services.AddScoped<ISocketService, SocketService>();

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
builder.Services.AddScoped<IUserService, UserService>();
// builder.Services.AddScoped<IWorkspaceAuthorizationService, WorkspaceAuthorizationService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();
// builder.Services.AddScoped<IConversationService, ConversationService>();
// builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ISlotService, SlotService>();
// builder.Services.AddScoped<IEventMemberService, EventMemberService>();

// Soft delete interceptor
// builder.Services.AddScoped<SoftDeleteInterceptor>();

// Utils Services
// builder.Services.AddScoped<IRepetitionService, RepetitionService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("DEV MODE");
    // Logger
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    // Swagger
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
}
else
{
    // TODO set the production logger
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Client");
app.UseHttpsRedirection();
app.UseExceptionHandler("/Api/Error");
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapGroup("Api/Auth").MapIdentityApi<User>().RequireRateLimiting("AuthPolicy");
app.MapControllers();
app.MapHub<GlobalHub>("/Api/Hub");

app.Run();
