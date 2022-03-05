using ChessEngineInterfacePOC.Interfaces;
using ChessEngineInterfacePOC.Models;
using ChessEngineInterfacePOC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<EngineConfig>(builder.Configuration.GetSection("EngineConfig"));
builder.Services.AddSingleton<IEngineService,EngineService>();
builder.Services.AddSingleton<IStockfishService, StockfishService>();
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