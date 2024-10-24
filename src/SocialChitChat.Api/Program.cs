using Asp.Versioning;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SocialChitChat.Api.Extensions;
using SocialChitChat.Api.Middleware;
using SocialChitChat.Business;
using SocialChitChat.Business.SignalR;
using SocialChitChat.DataAccess;
using SocialChitChat.DataAccess.Data;
using SocialChitChat.DataAccess.Identity;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

    builder.Services.AddCors();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerDocument();

    // register serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom
        .Configuration(builder.Configuration)
        .CreateLogger();

    builder.Host.UseSerilog();

    // setting connection string and register DbContext
    var defaultConnectionStringBuilder = new SqlConnectionStringBuilder
    {
        ConnectionString = builder.Configuration.GetConnectionString("DatingLoveAppConnectionString"),
        UserID = builder.Configuration["UserID"],
        Password = builder.Configuration["Password"]
    };

    builder.Services.AddDbContext<DatingLoveAppDbContext>(options =>
        options.UseSqlServer(defaultConnectionStringBuilder.ConnectionString));

    var identityConnectionStringBuilder = new SqlConnectionStringBuilder
    {
        ConnectionString = builder.Configuration.GetConnectionString("IdentityConnectionString"),
        UserID = builder.Configuration["UserID"],
        Password = builder.Configuration["Password"]
    };

    builder.Services.AddDbContext<DatingLoveAppIdentityDbContext>(options =>
        options.UseSqlServer(identityConnectionStringBuilder.ConnectionString));

    // register dependencies in other players
    builder.Services
        .AddBusiness()
        .AddDataAccess(builder.Configuration);

    // versioning the api
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
        options.AddApiVersionParametersWhenVersionNeutral = true;
    });
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerDocument();
    }

    //app.UseHttpsRedirection();

    using (IServiceScope serviceScope = app.Services.CreateScope())
    {
        using DatingLoveAppIdentityDbContext dbContext = serviceScope.ServiceProvider
            .GetRequiredService<DatingLoveAppIdentityDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    app.UseMiddleware<ExceptionHandlerMiddleware>();

    app.UseSerilogRequestLogging();

    app.UseCors(policy => policy
        .AllowAnyHeader()
        .AllowCredentials()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:4200", "http://datinglove.vutiendat3601.io.vn"));

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<PresenceHub>("hubs/presence");
    app.MapHub<MessageHub>("hubs/message");

    app.Run();
}