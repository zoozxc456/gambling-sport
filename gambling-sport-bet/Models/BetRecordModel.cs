using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using gambling_sport_bet;

namespace gamblingsportbet.Models
{
    


    public class BetRecordModel
    {
        [Column("member_id")]
        [Key]
        public Guid MemberId { get; set; }
        [Column("game_id")]
        [Key]
        public Guid GameId { get; set; }
        [Column("bet_time")]
        public DateTime BetTime { get; set; }
        [Column("handsel")]
        public decimal Handsel { get; set; }
        [Column("bet_result")]
        public string BetResult { get; set; }
        [Column("status")]
        public string Status { get; set; } = "pending";
        [Column("returnval")]
        public decimal ReturnVal { get; set; }

        //public BetRecordModel GrpcToBetRecordModel(Bet grpcBetRecord)
        //{
        //    var betRecord = new BetRecordModel
        //    {
        //        MemberId = Guid.Parse(grpcBetRecord.MemberId),
        //        GameId = Guid.Parse(grpcBetRecord.GameId),
        //        Handsel = Decimal.Parse(grpcBetRecord.Handsel),
        //        BetTime = DateTime.Parse(grpcBetRecord.BetTime),
        //        BetResult = grpcBetRecord.BetResult
        //    };
        //    return betRecord;
        //}

        //public Bet BetRecordModelToGrpc()
        //{
        //    var grpcBetRecord = new Bet
        //    {
        //        MemberId = this.MemberId.ToString(),
        //        GameId = this.GameId.ToString(),
        //        Handsel = this.Handsel.ToString(),
        //        BetTime = this.BetTime.ToString(),
        //        BetResult = this.BetResult.ToString()
        //    };
        //    return grpcBetRecord;
        //}
    }
}

