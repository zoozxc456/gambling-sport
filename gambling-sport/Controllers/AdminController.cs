using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gambling_sport.Models.ViewModels;

namespace gambling_sport.Controllers
{
    public class AdminController : Controller
    {
        private readonly GameProcessing.GameProcessingClient _gameClient;

        public AdminController(GameProcessing.GameProcessingClient gameClient)
        {
            _gameClient = gameClient;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _gameClient.GetGamesAsync(new GetGamesRequest());

            var sportGameList = games.Games.ToList().Select(game => new SportGameModel
            {
                GameId = game.GameId,
                Gamename = game.Gamename,
                Description = game.Description,
                HostTeamname = game.HostTeamname,
                HostWinRate = game.HostWinRate,
                GuestTeamname = game.GuestTeamname,
                GuestWinRate = game.GuestWinRate,
                IsLocked = game.IsLocked,
                Result = game.Result
            }).ToList();

            var viewModel = new SportGameModelListViewModel { SportGameList = sportGameList };
            return View(viewModel);
        }

        public IActionResult CreateNewGame()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame(SportGameModel model)
        {
            var grpcRequest = new CreateGameRequest
            {
                Game = new Game
                {
                    Gamename = model.Gamename,
                    Description = model.Description,
                    HostTeamname = model.HostTeamname,
                    HostWinRate = model.HostWinRate,
                    GuestTeamname = model.GuestTeamname,
                    GuestWinRate = model.GuestWinRate,
                    IsLocked = model.IsLocked
                }
            };

            var grpcResponse =  await _gameClient.CreateGameAsync(grpcRequest);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGame(SportGameModel model)
        {
            var grpcGame = new UpdateWinRateRequest
            {
                Game = new Game
                {
                    GameId = model.GameId,
                    Gamename = model.Gamename,
                    Description = model.Description,
                    HostTeamname = model.HostTeamname,
                    HostWinRate = model.HostWinRate,
                    GuestTeamname = model.GuestTeamname,
                    GuestWinRate = model.GuestWinRate,
                    IsLocked = model.IsLocked,
                    Result = model.Result
                }
            };
            var response = await _gameClient.UpdateWinRateAsync(grpcGame);
            return RedirectToAction("Index");
        }
    }
}