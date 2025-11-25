using WebApplication1.Dtos;
using WebApplication1.EndPoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGamesEndPoints();

app.Run();

