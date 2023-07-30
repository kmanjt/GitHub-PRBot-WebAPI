using GitHubBot.Services.Interfaces;
using GitHubBot.Services;
using GitHubBot.Models.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Configuration
builder.Host.ConfigureAppConfiguration((hostingContext, configuration) =>
{
    configuration.Sources.Clear();

    var env = hostingContext.HostingEnvironment;

    configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                              optional: true, reloadOnChange: true);

    if (env.IsDevelopment())
    {
        configuration.AddUserSecrets<Program>();
    }

    configuration.AddEnvironmentVariables();
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register config
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Register Services
builder.Services.AddTransient<IOpenAIService, OpenAIService>();
builder.Services.AddTransient<IGitHubService, GitHubService>();

// Configure logging
builder.Logging.AddConsole(options => options.IncludeScopes = true);
builder.Logging.SetMinimumLevel(LogLevel.Trace);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
