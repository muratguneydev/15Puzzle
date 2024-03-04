using FifteenPuzzle.Solvers.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

ServiceConfigurator.ConfigureServices(builder.Services, builder.Configuration);

builder.Services.AddControllers()
	.AddNewtonsoftJson();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();

//ServiceConfigurator.ConfigureServices(builder.Services, builder.Configuration);

builder.WebHost.UseUrls("http://localhost:6057");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();

//Make the Program class public using a partial class declaration:
public partial class Program { }
