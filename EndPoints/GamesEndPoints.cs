using WebApplication1.Dtos;

namespace WebApplication1.EndPoints;

public static class GamesEndPoints
{
        
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = new()
    {
        new GameDto(1, "The Legend of Code", "Adventure", 59.99m, new DateOnly(2023, 11, 15)),
        new GameDto(2, "Bug Hunter", "Action", 49.99m, new DateOnly(2024, 2, 20)),
        new GameDto(3, "Refactor Racer", "Racing", 39.99m, new DateOnly(2022, 8, 5))
    };

    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithParameterValidation();

        // Get /games
        group.MapGet("/", () => games);

        // Get /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(g => g.Id == id); // var = GameDto?
            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto createGameDto) =>
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

        group.MapPut("/{id}", (int id, UpdateGameDto updateGameDto) =>
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

        group.MapDelete("/{id}", (int id) =>
        {
            var gameIndex = games.FindIndex(g => g.Id == id);
            if (gameIndex == -1)
            {
                return Results.NotFound();
            }

            games.RemoveAt(gameIndex);
            return Results.NoContent();
        });

        return group;
    }
}
