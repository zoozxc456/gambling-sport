using System;
namespace gambling_sport.Models.ViewModels
{
	public class UserBetFormViewModel
	{
		public Guid GameId { get; set; }
		public string BetResult { get; set; } = string.Empty;
		public string Handsel { get; set; } = string.Empty;
	}

	public class BetRecordModel
	{
        public string GameId { get; set; } = string.Empty;
        public string Gamename { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string HostTeamname { get; set; } = string.Empty;
        public double HostWinRate { get; set; }
        public string GuestTeamname { get; set; } = string.Empty;
        public double GuestWinRate { get; set; }
		public string BetResult { get; set; } = string.Empty;
		public decimal Handsel { get; set; }
		public string Status { get; set; } = string.Empty;
		public decimal ReturnVal { get; set; }
    }

	public class BetRecordsViewModel
	{
		public List<BetRecordModel>? Records { get; set; }
	}
}

