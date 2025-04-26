using MongoDB.Driver;
using Microsoft.Extensions.Options;
using KindQuestAPI.Configuration;
using KindQuestAPI.DAL;
using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.DAL.Repositories;
using KindQuestAPI.Services;
using KindQuestAPI.BackgroundServices;
using TaskModel = KindQuestAPI.Models.Task;

var builder = WebApplication.CreateBuilder(args);

// Add controller support
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- MongoDB configuration and registration ---
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    
    if (string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new InvalidOperationException("MongoDB ConnectionString in MongoDbSettings is not configured.");
    }
    
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    
    if (string.IsNullOrEmpty(settings.DatabaseName))
    {
        throw new InvalidOperationException("MongoDB DatabaseName in MongoDbSettings is not configured.");
    }
     
    return client.GetDatabase(settings.DatabaseName);
});

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICreatureRepository, CreatureRepository>();
builder.Services.AddScoped<IQuestRepository, QuestRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Register application services
builder.Services.AddScoped<QuestService>();

// Register background services
builder.Services.AddHostedService<DailyQuestSchedulerService>();

// Configure CORS for frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Configure CORS
app.UseCors("AllowAll");

app.UseAuthorization();

// Map controllers
app.MapControllers();

// Log and run
app.Logger.LogInformation("KindQuest API starting up.");
app.Run();