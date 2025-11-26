using System;
using WebApplication1.Dtos;
using WebApplication1.Entities;

namespace WebApplication1.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDto createGameDto)
    {
        return new Game
        {
            Name = createGameDto.Name,
            GenreId = createGameDto.GenreId,
            Price = createGameDto.Price,
            ReleaseDate = createGameDto.ReleaseDate
        };
    }
    public static Game ToEntity(this UpdateGameDto createGameDto, int id)
    {
        return new Game
        {
            Id = id,
            Name = createGameDto.Name,
            GenreId = createGameDto.GenreId,
            Price = createGameDto.Price,
            ReleaseDate = createGameDto.ReleaseDate
        };
    }
    public static GameSummaryDto ToGameSummaryDTO(this Game game)
    {
        return new GameSummaryDto(
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate
        );
    }

    public static GameDetailsDto ToGameDetailsDTO(this Game game)
    {
        return new GameDetailsDto(
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
    }
}
