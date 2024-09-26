
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Repositories;
using BachelorTherasoftDotnetApi.src.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySqlConnector;       
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
        Description = "An ASP.NET Core Web API for collaborativ agenda",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    // Ajoute les commentaire du code dans le swagger 
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<MySqlDbContext>( 
    options => options.UseMySql(
        new MySqlConnection(builder.Configuration.GetConnectionString("MySQL")),
        new MySqlServerVersion(new Version(8, 0, 38)),
        options => options.EnableRetryOnFailure()
));
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

// Email service
//builder.Services.AddTransient<IEmailSender, EmailSender>();
//builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// Authentication service
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Custom Repositories
builder.Services.AddScoped<WorkspaceRepository>();
builder.Services.AddScoped<WorkspaceRoleRepository>();
builder.Services.AddScoped<LocationRepository>();
builder.Services.AddScoped<AreaRepository>();
builder.Services.AddScoped<RoomRepository>();
builder.Services.AddScoped<EventRepository>();
builder.Services.AddScoped<EventCategoryRepository>();
// Custom Services
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IWorkspaceRoleService, WorkspaceRoleService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventCategoryService, EventCategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapIdentityApi<User>();
// TODO a delete lors de l'auth controller et auth service
app.MapPost("/register2", async (
    RegisterRequestDto request,
    UserManager<User> userManager) =>
{
    var user = new User
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

app.Run();
