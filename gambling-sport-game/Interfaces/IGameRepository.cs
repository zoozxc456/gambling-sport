using System;
using gamblingsportgame.Models;

namespace gamblingsportgame.Interfaces
{
	public interface IGameRepository
	{
		public List<GameModel> GetGamesTop50();
		public GameModel GetGameById(Guid GameId);
		public bool UpdateWinRate(GameModel game);
		public bool CreateGame(GameModel game);
	}
}

