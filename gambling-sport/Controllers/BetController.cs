using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gambling_sport;
using gambling_sport.Models.ViewModels;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace gambling_sport.Controllers
{
    public class BetController : Controller
    {
        private readonly GameProcessing.GameProcessingClient _gameProcessingClient;
        private readonly BetProccessing.BetProccessingClient _betProccessingClient;
        private readonly Member.MemberClient _memberClient;
        private readonly ISession _session;

        public BetController(GameProcessing.GameProcessingClient gameProcessingClient,
            BetProccessing.BetProccessingClient betProccessingClient,
            Member.MemberClient memberClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _gameProcessingClient = gameProcessingClient;
            _betProccessingClient = betProccessingClient;
            _memberClient = memberClient;
            _session = httpContextAccessor.HttpContext.Session;
        }

        // GET: /<controller>/
        [HttpPost]
        public async Task<IActionResult> Index(string GameId)
        {
            var result = await _gameProcessingClient.GetGameByIdAsync(new GetGameByIdRequest { GameId = GameId });
            var grpcGame = result.Game;
            var game = new SportGameModel
            {
                GameId = grpcGame.GameId,
                Gamename = grpcGame.Gamename,
                Description = grpcGame.Description,
                HostTeamname = grpcGame.HostTeamname,
                HostWinRate = grpcGame.HostWinRate,
                GuestTeamname = grpcGame.GuestTeamname,
                GuestWinRate = grpcGame.GuestWinRate,
                IsLocked = grpcGame.IsLocked
            };

            var viewModel = new LobbyViewModel
            {
                SportGameList = new List<SportGameModel> { game }
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserBet(UserBetFormViewModel viewModel)
        {
            var betRequest = new CreateBetRecordRequest
            {
                NewBetRecord = new Bet
                {
                    MemberId = _session.GetString("id"),
                    GameId = viewModel.GameId.ToString(),
                    BetResult = viewModel.BetResult,
                    Handsel = viewModel.Handsel
                }
            };

            var betResponse = await _betProccessingClient.CreateBetRecordAsync(betRequest);

            var updateBalanceRequest = new UpdateBalanceRequest
            {
                MemberId = _session.GetString("id"),
                UserBetHandsel = viewModel.Handsel
            };
            var updateBalanceResponse = await _memberClient.UpdateBalanceAsync(updateBalanceRequest);

            return RedirectToAction("Index", "Lobby");
        }

        [HttpPost]
        public IActionResult Settle(string gameId)
        {
            var game = _gameProcessingClient.GetGameById(new GetGameByIdRequest { GameId = gameId });
            var result = game.Game.Result;
            var rate = game.Game.HostTeamname == result ? game.Game.HostWinRate : game.Game.GuestWinRate;

            var settleBetRecordResponse = _betProccessingClient.SettleBetRecord(new SettleBetRecordRequest
            {
                GameId = gameId,
                Result = result,
                Rate = rate
            });

            var grpcBetRecords = settleBetRecordResponse.SettleRecords;
            var memberSettleRequest = new SettleRequest();

            grpcBetRecords
                 .Select(record => new SettleModel { MemberId = record.MemberId, Returnval = record.ReturnVal })
                 .ToList().ForEach(x =>
                 {
                     memberSettleRequest.Members.Add(x);
                 });

            _memberClient.Settle(memberSettleRequest);

            return Ok();
        }
    }
}

