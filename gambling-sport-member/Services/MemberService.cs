using Grpc.Core;
using gambling_sport_member;
using gamblingsportmember.Models;
using gamblingsportmember.Interfaces;
using gamblingsportmember.Helpers;
namespace gambling_sport_member.Services
{
    public class MemberService : Member.MemberBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IPasswordUtility _passwordUtility;
        private readonly JwtHelper _jwtHelper;
        public MemberService(IMemberRepository memberRepository, IPasswordUtility passwordUtility, JwtHelper jwtHelper)
        {
            _memberRepository = memberRepository;
            _passwordUtility = passwordUtility;
            _jwtHelper = jwtHelper;
        }
        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var account = request.Account;
            var rawPassword = request.Password;

            var member = _memberRepository.GetUserByAccount(account);
            if (member == null)
            {
                return Task.FromResult(new LoginResponse { Token = "" });
            }


            var dbPassword = member.Password;

            if (_passwordUtility.VerifyPassword(rawPassword, member.Password))
            {
                var token = _jwtHelper.GenerateToken(member);
                return Task.FromResult(new LoginResponse
                {
                    Token = token,
                    Role = member.Role,
                    Username = member.Username,
                    Id = member.Id.ToString()
                });

            }

            return Task.FromResult(new LoginResponse { Token = "" });

        }

        public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var hashedPassword = _passwordUtility.HashPassword(request.Password);

            var member = new MemberModel
            {
                Account = request.Account,
                Password = hashedPassword,
                Username = request.Username,
                Email = request.Email
            };

            var isSuccess = _memberRepository.CreateOneNewMember(member);

            return Task.FromResult(new RegisterResponse
            {
                Message = isSuccess ?
                    "Create New Member Success" :
                    "Create New Member Failed"
            });
        }

        public override Task<GetAccountResponse> GetAccount(GetUserInfoRequest request, ServerCallContext context)
        {
            var token = request.Token;
            var account = _jwtHelper.DecodeToken(token).Account;
            return Task.FromResult(new GetAccountResponse { Account = account });
        }

        public override Task<GetMemberByIdResponse> GetMemberById(GetMemberByIdRequest request, ServerCallContext context)
        {
            var memberId = Guid.Parse(request.MemberId);

            var member = _memberRepository.GetUserById(memberId);

            var response = new GetMemberByIdResponse
            {
                Account = member.Account,
                Username = member.Username,
                MemberId = member.Id.ToString(),
                Email = member.Email,
                Role = member.Role,
                Balance = member.Balance.ToString()
            };

            return Task<GetMemberByIdResponse>.FromResult(response);
        }

        public override Task<UpdateBalanceResponse> UpdateBalance(UpdateBalanceRequest request, ServerCallContext context)
        {
            var memberId = Guid.Parse(request.MemberId);
            var userBetHandsel = Decimal.Parse(request.UserBetHandsel);

            var member = _memberRepository.GetUserById(memberId);
            var newBalance = member.Balance - userBetHandsel;
            var root = _memberRepository.GetUserByAccount("root");
            var rootNewBalance = root.Balance + userBetHandsel;

            var isSuccess = _memberRepository.UpdateUserBalance(memberId, newBalance) &&
                _memberRepository.UpdateRootBalance(rootNewBalance);

            var updatedBalance = _memberRepository.GetUserBalanceById(memberId);

            return Task<UpdateBalanceResponse>.FromResult(
                new UpdateBalanceResponse
                {
                    IsSuccess = isSuccess,
                    UpdatedBalance = updatedBalance.ToString()
                });
        }


        //public override



        public override Task<SettleResponse> Settle(SettleRequest request, ServerCallContext context)
        {
            var isSuccess = true;
            request.Members.ToList().ForEach(grpcMember =>
            {
                var memberId = Guid.Parse(grpcMember.MemberId);
                var returnVal = decimal.Parse(grpcMember.Returnval);
                isSuccess &= _memberRepository.Settle(memberId, returnVal);
            });

            return Task<SettleResponse>.FromResult(new SettleResponse { IsSuccess = isSuccess });
        }
    }
}

