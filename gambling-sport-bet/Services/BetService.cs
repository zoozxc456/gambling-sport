using Grpc.Core;
using System;
using gambling_sport_bet;
using gamblingsportbet.Interfaces;
using gamblingsportbet.Models;
namespace gamblingsportbet.Services
{
    public class BetService : BetProccessing.BetProccessingBase
    {
        private readonly IBetRecordRepository _betRecordRepository;

        public BetService(IBetRecordRepository betRecordRepository)
        {
            _betRecordRepository = betRecordRepository;
        }

        public override Task<CreateBetRecordResponse> CreateBetRecord(CreateBetRecordRequest request, ServerCallContext context)
        {
            var grpcBetRecord = request.NewBetRecord;
            grpcBetRecord.BetTime = DateTime.Now.ToString();

            var betRecord = new BetRecordModel
            {
                MemberId = Guid.Parse(grpcBetRecord.MemberId),
                GameId = Guid.Parse(grpcBetRecord.GameId),
                Handsel = Decimal.Parse(grpcBetRecord.Handsel),
                BetTime = DateTime.Parse(grpcBetRecord.BetTime),
                BetResult = grpcBetRecord.BetResult
            };

            var isSuccess = _betRecordRepository.CreateBetRecord(betRecord);
            return Task<CreateBetRecordResponse>.FromResult(new CreateBetRecordResponse { IsSuccess = isSuccess });
        }

        public override Task<GetBetRecordsResponse> GetBetRecordsById(GetBetRecordsByIdRequest request, ServerCallContext context)
        {
            var grpcMemberId = request.MemberId;
            var memberId = Guid.Parse(grpcMemberId);
            var records = _betRecordRepository.GetBetRecordsById(memberId);

            var grpcResponse = new GetBetRecordsResponse();

            records.ForEach(record =>
            {
                var grpcRecord = new Bet
                {
                    MemberId = record.MemberId.ToString(),
                    GameId = record.GameId.ToString(),
                    BetTime = record.BetTime.ToString(),
                    BetResult = record.BetResult.ToString(),
                    Handsel = record.Handsel.ToString(),
                    Status = record.Status,
                    ReturnVal = record.ReturnVal.ToString()
                };
                grpcResponse.BetRecords.Add(grpcRecord);
            });

            return Task<GetBetRecordsResponse>.FromResult(grpcResponse);
        }

        public override Task<SettleBetRecordResponse> SettleBetRecord(SettleBetRecordRequest request, ServerCallContext context)
        {
            var gameId = Guid.Parse(request.GameId);
            var result = request.Result;
            var rate = request.Rate;

            var isSuccess = _betRecordRepository.SettleBetRecord(gameId, result, rate);

            var records = _betRecordRepository.GetBetRecordsByGameId(gameId);
            var response = new SettleBetRecordResponse();

            records.ForEach(record =>
            {
                var grpcRecord = new Bet
                {
                    GameId = record.GameId.ToString(),
                    BetResult = record.BetResult,
                    BetTime = record.BetTime.ToString(),
                    MemberId = record.MemberId.ToString(),
                    Status = record.Status,
                    Handsel = record.Handsel.ToString(),
                    ReturnVal = record.ReturnVal.ToString()
                };
                response.SettleRecords.Add(grpcRecord);
            });

            return Task<SettleBetRecordResponse>.FromResult(response);
        }
    }
}

