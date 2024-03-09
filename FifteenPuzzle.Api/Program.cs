using FifteenPuzzle.Api;
//using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Services.AddHttpLogging(o => { });

// Add services to the container.
builder.Services.AddControllers()
	.AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton<BoardSessionRepository>();

builder.WebHost.UseUrls("http://localhost:5057");

var app = builder.Build();
app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();
//app.UseDeveloperExceptionPage();

app.MapControllers();

app.Run();

//Make the Program class public using a partial class declaration:
public partial class Program { }