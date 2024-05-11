using System;
namespace gambling_sport.Models.ViewModels
{
    public class SportGameModel
    {
        public string GameId { get; set; } = string.Empty;
        public string Gamename { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string HostTeamname { get; set; } = string.Empty;
        public double HostWinRate { get; set; }
        public string GuestTeamname { get; set; } = string.Empty;
        public double GuestWinRate { get; set; }
        public bool IsLocked { get; set; }
        public string Result { get; set; } = string.Empty;
    }

    public class SportGameModelListViewModel
    {
        public List<SportGameModel>? SportGameList { get; set; }
    }
}

