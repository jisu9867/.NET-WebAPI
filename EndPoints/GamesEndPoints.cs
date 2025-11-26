using System.Threading.Tasks;
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
        group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDTO())
                .AsNoTracking()
                .ToListAsync());

        // Get /games/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);
            return game is not null ? Results.Ok(game.ToGameDetailsDTO()) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        group.MapPost("/", async (CreateGameDto createGameDto, GameStoreContext dbContext) =>
        {
            Game game = createGameDto.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDTO());
        });

        group.MapPut("/{id}", async (int id, UpdateGameDto updateGameDto, GameStoreContext dbContext) =>
        {
            var exisitingGame = await dbContext.Games.FindAsync(id);
            if (exisitingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(exisitingGame).CurrentValues.SetValues(updateGameDto.ToEntity(id));
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });

        return group;
    }
}
