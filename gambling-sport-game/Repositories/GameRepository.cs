using System;
using gamblingsportgame.Interfaces;
using gamblingsportgame.Models;

namespace gamblingsportgame.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDbContext _context;

        public GameRepository(GameDbContext context)
        {
            _context = context;
        }

        public bool CreateGame(GameModel game)
        {
            try
            {
                _context.Add(game);
                var affectRows = _context.SaveChanges();
                return affectRows == 1;
            }
            catch
            {
                throw;
            }
        }

        public GameModel GetGameById(Guid GameId)
        {
            return _context.Games.FirstOrDefault(game => game.GameId == GameId);
        }

        public List<GameModel> GetGamesTop50()
        {
            try
            {
                var games = _context.Games.Take(50).ToList();
                return games;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateWinRate(GameModel game)
        {
            try
            {
                _context.Games.Update(game);
                var affectRows = _context.SaveChanges();
                return affectRows == 1;
            }
            catch
            {
                throw;
            }
        }
    }
}

