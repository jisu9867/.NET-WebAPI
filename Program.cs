using WebApplication1.Data;
using WebApplication1.EndPoints;

var builder = WebApplication.CreateBuilder(args);


// migration 전
var connectionString = builder.Configuration.GetConnectionString("GameStore") 
    ?? "Data Source=GameStore.db";
builder.Services.AddSqlite<GameStoreContext>(connectionString);

var app = builder.Build();

app.MapGamesEndPoints();
app.MapGenresEndPoints();

await app.MigrateDbAsync();

app.Run();

