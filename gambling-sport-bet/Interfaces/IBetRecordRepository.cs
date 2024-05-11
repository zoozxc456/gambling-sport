using System;
using gamblingsportbet.Models;

namespace gamblingsportbet.Interfaces
{
	public interface IBetRecordRepository
	{
		public List<BetRecordModel> GetBetRecordsById(Guid memberId);
		public List<BetRecordModel> GetBetRecordsByGameId(Guid gameId);
		public bool CreateBetRecord(BetRecordModel betRecord);
		public bool SettleBetRecord(Guid gameId, string result,double rate);
	}
}

