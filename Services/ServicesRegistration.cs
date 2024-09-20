//using BachelorTherasoftDotnetApi.Models;
//using BachelorTherasoftDotnetApi.Databases;
//using BachelorTherasoftDotnetApi.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Routing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using MySqlConnector;

//namespace BachelorTherasoftDotnetApi.Services;

//public static class ServicesRegistration
//{
//    // 
//    public static void AddMySqlDbContext(this IServiceCollection services, IConfiguration configuration)
//    {
//        services.AddDbContext<MySqlDbContext>(
//            options => options.UseMySql(
//                new MySqlConnection(configuration.GetConnectionString("MySQL")),
//                new MySqlServerVersion(new Version(8, 0, 38)),
//                options => options.EnableRetryOnFailure()
//            ));
//    }

//    public static void AddIdentity(this IServiceCollection services)
//    {
//        services.AddIdentity<User, Role>(options =>
//        {
//            // options.SignIn.RequireConfirmedEmail = true;
//            options.User.RequireUniqueEmail = true;
//            // options.User.AllowedUserNameCharacters = "azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN0123456789";
//            options.Password.RequireNonAlphanumeric = true;
//            options.Password.RequiredLength = 10;
//            options.Password.RequireDigit = true;
//            options.Password.RequireLowercase = true;
//            options.Password.RequireUppercase = true;
//            options.Lockout.MaxFailedAccessAttempts = 5;
//        })
//        .AddEntityFrameworkStores<MySqlDbContext>()
//        .AddDefaultTokenProviders()
//        .AddApiEndpoints();
//    }

//    public static void AddIdentityApiEndpoints(this IApplicationBuilder applicationBuilder)
//    {
//        applicationBuilder.UseEndpoints(endpoints =>
//        {
//            endpoints.MapIdentityApi<User>();
//        });
//    }

//    public static void AddWebCors(this IServiceCollection service)
//    {
//        service.AddCors(Options => Options.AddPolicy("Angular", policy =>
//            policy.WithOrigins("http://localhost:4200/")
//                .AllowAnyHeader()
//                .AllowAnyMethod()
//        ));
//    }

//    public static void UseWebCors(this IApplicationBuilder builder)
//    {
//        builder.UseCors("Angular");
//    }

//    public static void AddEmailSender(this IServiceCollection service, IConfiguration configuration)
//    {
//        service.AddTransient<IEmailSender, EmailSender>();
//        service.Configure<AuthMessageSenderOptions>(configuration);
//    }
//    // test
//    public static void AddAuth(this IServiceCollection service)
//    {
//        service.AddAuthentication(options =>
//        {
//            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//        }).AddBearerToken();
//    }
//}
