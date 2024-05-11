using System;
using gamblingsportmember.Models;
namespace gamblingsportmember.Interfaces
{
	public interface IMemberRepository
	{
		public MemberModel GetUserById(Guid id);
        public MemberModel? GetUserByAccount(string account);
		public decimal GetUserBalanceById(Guid id);
		public bool CreateOneNewMember(MemberModel member);
		public bool UpdateUserBalance(Guid id, decimal newBalance);
		public bool UpdateRootBalance(decimal newBalance);
		public bool Settle(Guid memberId, decimal returnVal);

    }
}

