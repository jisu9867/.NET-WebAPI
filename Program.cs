using WebApplication1.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = new()
{
    new GameDto(1, "The Legend of Code", "Adventure", 59.99m, new DateOnly(2023, 11, 15)),
    new GameDto(2, "Bug Hunter", "Action", 49.99m, new DateOnly(2024, 2, 20)),
    new GameDto(3, "Refactor Racer", "Racing", 39.99m, new DateOnly(2022, 8, 5))
};

// Get /games
app.MapGet("/games", () => games);

// Get /games/{id}
app.MapGet("/games/{id}", (int id) =>
{
    var game = games.FirstOrDefault(g => g.Id == id);
    return game is not null ? Results.Ok(game) : Results.NotFound();
}).WithName(GetGameEndpointName);

app.MapPost("/games", (CreateGameDto createGameDto) =>
{
    var newId = games.Max(g => g.Id) + 1;
    var newGame = new GameDto(
        newId,
        createGameDto.Name,
        createGameDto.Genre,
        createGameDto.Price,
        createGameDto.ReleaseDate
    );
    games.Add(newGame);
    //return Results.Created($"/games/{newId}", newGame);   //Created, CreatedAtRoute의 용도는?
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = newId }, newGame);
});

app.MapPut("/games/{id}", (int id, UpdateGameDto updateGameDto) =>
{
    var gameIndex = games.FindIndex(g => g.Id == id);
    if (gameIndex == -1)
    {
        return Results.NotFound();
    }

    var updatedGame = new GameDto(
        id,
        updateGameDto.Name,
        updateGameDto.Genre,
        updateGameDto.Price,
        updateGameDto.ReleaseDate
    );
    games[gameIndex] = updatedGame;

    return Results.NoContent();
});

app.MapDelete("/games/{id}", (int id) =>
{
    var gameIndex = games.FindIndex(g => g.Id == id);
    if (gameIndex == -1)
    {
        return Results.NotFound();
    }

    games.RemoveAt(gameIndex);
    return Results.NoContent();
});



app.Run();

