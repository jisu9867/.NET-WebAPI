using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Entities;
using WebApplication1.Mapping;

namespace WebApplication1.EndPoints;

public static class GamesEndPoints
{
        
    const string GetGameEndpointName = "GetGame";
    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithParameterValidation();

        // Get /games
        group.MapGet("/", (GameStoreContext dbContext) => 
            dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDTO())
                .AsNoTracking());

        // Get /games/{id}
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);
            return game is not null ? Results.Ok(game.ToGameDetailsDTO()) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto createGameDto, GameStoreContext dbContext) =>
        {
            Game game = createGameDto.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDTO());
        });

        group.MapPut("/{id}", (int id, UpdateGameDto updateGameDto, GameStoreContext dbContext) =>
        {
            var exisitingGame = dbContext.Games.Find(id);
            if (exisitingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(exisitingGame).CurrentValues.SetValues(updateGameDto.ToEntity(id));
            dbContext.SaveChanges();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            dbContext.Games.Where(g => g.Id == id).ExecuteDelete();
            return Results.NoContent();
        });

        return group;
    }
}
