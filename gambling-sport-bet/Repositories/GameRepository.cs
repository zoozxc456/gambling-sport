using System;
using gamblingsportbet.Interfaces;
using gamblingsportbet.Models;

namespace gamblingsportbet.Repositories
{
    public class BetRecordRepository : IBetRecordRepository
    {
        private readonly BetDbContext _context;

        public BetRecordRepository(BetDbContext context)
        {
            _context = context;
        }

        public bool CreateBetRecord(BetRecordModel betRecord)
        {
            try
            {
                _context.BetRecords.Add(betRecord);
                var affectRows = _context.SaveChanges();
                return affectRows == 1;
            }
            catch
            {
                throw;
            }
        }

        public List<BetRecordModel> GetBetRecordsByGameId(Guid gameId)
        {
            return _context.BetRecords.Where(record => record.GameId == gameId && record.ReturnVal > decimal.Zero).ToList();
        }

        public List<BetRecordModel> GetBetRecordsById(Guid memberId)
        {
            try
            {
                var records = _context.BetRecords
                    .Where(record => record.MemberId == memberId).ToList();
                return records;
            }
            catch
            {
                throw;
            }
        }

        public bool SettleBetRecord(Guid gameId, string result, double rate)
        {
            _context.BetRecords
                .Where(record => record.GameId == gameId)
                .ToList()
                .ForEach(record =>
                {
                    record.Status = "settled";
                    var returnValue = record.BetResult == result ?
                                      record.Handsel * ((decimal)rate) :
                                      decimal.Zero;
                    record.ReturnVal = returnValue;
                });
            var affectRows = _context.SaveChanges();
            return affectRows == 1;
        }
    }
}

