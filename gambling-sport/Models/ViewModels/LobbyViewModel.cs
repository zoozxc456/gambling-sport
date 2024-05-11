using System;
namespace gambling_sport.Models.ViewModels
{
	public class LobbyViewModel
	{
        public List<SportGameModel>? SportGameList { get; set; }
        public string Username { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}

