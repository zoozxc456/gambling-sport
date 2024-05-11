using System;
using Grpc.Core;
using gambling_sport_game;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Google.Protobuf;
using gamblingsportgame.Interfaces;
using gamblingsportgame.Models;

namespace gambling_sport_game.Services;


public class GameService : GameProcessing.GameProcessingBase
{
    private readonly ILogger<GameService> _logger;
    private readonly IGameRepository _gameRepository;

    public GameService(ILogger<GameService> logger, IGameRepository gameRepository)
    {
        _logger = logger;
        _gameRepository = gameRepository;
    }

    public override Task<GetGamesResponse> GetGames(GetGamesRequest request, ServerCallContext context)
    {
        var games = _gameRepository.GetGamesTop50();

        var response = new GetGamesResponse();

        games.ForEach(game =>
        {
            var grpcGame = new Game
            {
                GameId = game.GameId.ToString(),
                Gamename = game.Gamename,
                Description = game.Description,
                HostTeamname = game.HostTeamname,
                HostWinRate = game.HostWinRate,
                GuestTeamname = game.GuestTeamname,
                GuestWinRate = game.GuestWinRate,
                IsLocked = game.IsLocked,
                Result = game.Result
            };
            response.Games.Add(grpcGame);
        });

        return Task<GetGamesResponse>.FromResult(response);
    }

    public override Task<GetGameByIdResponse> GetGameById(GetGameByIdRequest request, ServerCallContext context)
    {
        var grpcGameId = request.GameId;
        var gameId = Guid.Parse(grpcGameId);
        var game = _gameRepository.GetGameById(gameId);

        var response = new GetGameByIdResponse
        {
            Game = new Game
            {
                GameId = game.GameId.ToString(),
                Gamename = game.Gamename,
                Description = game.Description,
                HostTeamname = game.HostTeamname,
                HostWinRate = game.HostWinRate,
                GuestTeamname = game.GuestTeamname,
                GuestWinRate = game.GuestWinRate,
                IsLocked = game.IsLocked,
                Result = game.Result
            }
        };
        return Task<GetGameByIdResponse>.FromResult(response);
    }

    public override Task<UpdateWinRateResponse> UpdateWinRate(UpdateWinRateRequest request, ServerCallContext context)
    {
        var grpcGame = request.Game;

        var gameModel = new GameModel
        {
            GameId = Guid.Parse(grpcGame.GameId),
            Gamename = grpcGame.Gamename,
            Description = grpcGame.Description,
            HostTeamname = grpcGame.HostTeamname,
            HostWinRate = grpcGame.HostWinRate,
            GuestTeamname = grpcGame.GuestTeamname,
            GuestWinRate = grpcGame.GuestWinRate,
            IsLocked = grpcGame.IsLocked,
            Result = grpcGame.Result
        };

        var isSuccess = _gameRepository.UpdateWinRate(gameModel);

        return Task<UpdateWinRateResponse>.FromResult(
            new UpdateWinRateResponse { IsSuccess = isSuccess });
    }

    public override Task<CreateGameResponse> CreateGame(CreateGameRequest request, ServerCallContext context)
    {
        var grpcGame = request.Game;
        var gameModel = new GameModel
        {
            Gamename = grpcGame.Gamename,
            Description = grpcGame.Description,
            HostTeamname = grpcGame.HostTeamname,
            HostWinRate = grpcGame.HostWinRate,
            GuestTeamname = grpcGame.GuestTeamname,
            GuestWinRate = grpcGame.GuestWinRate,
            IsLocked = grpcGame.IsLocked
        };

        var isSuccess = _gameRepository.CreateGame(gameModel);

        return Task<CreateGameResponse>.FromResult(
            new CreateGameResponse { IsSuccess = isSuccess });
    }

}


