using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gambling_sport.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace gambling_sport.Controllers
{
    public class RecordController : Controller
    {
        private readonly ISession _session;
        private readonly BetProccessing.BetProccessingClient _betProccessingClient;
        private readonly GameProcessing.GameProcessingClient _gameProcessingClient;

        public RecordController(IHttpContextAccessor httpContextAccessor,
            BetProccessing.BetProccessingClient betProccessingClient,
            GameProcessing.GameProcessingClient gameProcessingClient)
        {
            _session = httpContextAccessor.HttpContext.Session;
            _betProccessingClient = betProccessingClient;
            _gameProcessingClient = gameProcessingClient;
        }

        public async Task<IActionResult> Index()
        {
            var memberId = _session.GetString("id");
            var getBetRecordsRequest = new GetBetRecordsByIdRequest { MemberId = memberId };

            var records = new List<BetRecordModel>();

            var grpcRecords = await _betProccessingClient.GetBetRecordsByIdAsync(getBetRecordsRequest);
            grpcRecords.BetRecords.ToList().ForEach( grpcRecord =>
            {
                var gameRequest = new GetGameByIdRequest { GameId = grpcRecord.GameId };
                var grpcResponse = _gameProcessingClient.GetGameById(gameRequest);
                var grpcGame = grpcResponse.Game;

                var record = new BetRecordModel
                {
                    GameId = grpcGame.GameId,
                    Gamename = grpcGame.Gamename,
                    Description = grpcGame.Description,
                    HostTeamname = grpcGame.HostTeamname,
                    HostWinRate = grpcGame.HostWinRate,
                    GuestTeamname = grpcGame.GuestTeamname,
                    GuestWinRate = grpcGame.GuestWinRate,
                    BetResult = grpcRecord.BetResult,
                    Handsel = Decimal.Parse(grpcRecord.Handsel),
                    Status = grpcRecord.Status,
                    ReturnVal = Decimal.Parse(grpcRecord.ReturnVal)
                };

                records.Add(record);
            });

            var viewModel = new BetRecordsViewModel
            {
                Records = records
            };

            return View(viewModel);
        }
    }
}

