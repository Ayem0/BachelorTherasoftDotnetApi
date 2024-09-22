using BachelorTherasoftDotnetApi.Databases;
using BachelorTherasoftDotnetApi.Dtos;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Repositories;
using BachelorTherasoftDotnetApi.Services;

//using BachelorTherasoftDotnetApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using System.Text;

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

//builder.Services.AddTransient<IEmailSender, EmailSender>();
//builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();

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
app.MapPost("/register2", async (
    RegisterRequestDto request,
    UserManager<User> userManager,
    IPasswordHasher<User> passwordHasher) =>
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
