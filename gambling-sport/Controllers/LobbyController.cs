using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gambling_sport.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace gambling_sport.Controllers
{
    public class LobbyController : Controller
    {
        private readonly ILogger<LobbyController> _logger;
        private readonly ISession _session;
        private readonly GameProcessing.GameProcessingClient _gameProcessingClient;
        private readonly Member.MemberClient _memberClient;
        public LobbyController(ILogger<LobbyController> logger,
            IHttpContextAccessor httpContextAccessor,
            GameProcessing.GameProcessingClient gameProcessingClient,
            Member.MemberClient memberClient)
        {
            _logger = logger;
            _session = httpContextAccessor.HttpContext.Session;
            _gameProcessingClient = gameProcessingClient;
            _memberClient = memberClient;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var role = _session.GetString("role") ?? "";
            if (role == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else if (role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            var games = await _gameProcessingClient.GetGamesAsync(new GetGamesRequest());

            var sportGameList = games.Games.ToList().Select(game => new SportGameModel
            {
                GameId = game.GameId,
                Gamename = game.Gamename,
                Description = game.Description,
                HostTeamname = game.HostTeamname,
                HostWinRate = game.HostWinRate,
                GuestTeamname = game.GuestTeamname,
                GuestWinRate = game.GuestWinRate,
                IsLocked = game.IsLocked
            }).ToList();

            var grpcMemberRequest = await _memberClient.GetMemberByIdAsync(new GetMemberByIdRequest
            {
                MemberId = _session.GetString("id")
            });

            var balance = grpcMemberRequest.Balance;
            var username = grpcMemberRequest.Username;

            var viewModel = new LobbyViewModel
            {
                SportGameList = sportGameList,
                Balance = Decimal.Parse(balance),
                Username = username
            };
            return View(viewModel);
        }
    }
}

