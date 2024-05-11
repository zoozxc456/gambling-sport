using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace gamblingsportgame.Models
{
	public class GameModel
	{
        [Column("game_id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GameId { get; set; }
        public string Gamename { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column("host_teamname")]
        public string HostTeamname { get; set; } = string.Empty;
        [Column("host_win_rate")]
        public double HostWinRate { get; set; }
        [Column("guest_teamname")]
        public string GuestTeamname { get; set; } = string.Empty;
        [Column("guest_win_rate")]
        public double GuestWinRate { get; set; }
        [Column("isLocked")]
        public bool IsLocked { get; set; }
        public string Result { get; set; } = string.Empty;
    }
}

