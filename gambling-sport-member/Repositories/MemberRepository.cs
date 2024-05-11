using System;
using gamblingsportmember.Interfaces;
using gamblingsportmember.Models;

namespace gamblingsportmember.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MemberDbContext _dbContext;
        public MemberRepository(MemberDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateOneNewMember(MemberModel member)
        {
            _dbContext.Members.Add(member);
            var affectRows = _dbContext.SaveChanges();
            return affectRows == 1;
        }

        public decimal GetUserBalanceById(Guid id)
        {
            return _dbContext.Members.FirstOrDefault(member => member.Id == id).Balance;
        }

        public MemberModel? GetUserByAccount(string account)
        {
            return _dbContext.Members.FirstOrDefault(user => user.Account == account);
        }

        public MemberModel GetUserById(Guid userId)
        {
            return _dbContext.Members.FirstOrDefault(member => member.Id == userId);
        }

        public bool Settle(Guid memberId, decimal returnVal)
        {
            var user = _dbContext.Members.FirstOrDefault(x => x.Id == memberId);
            user.Balance += returnVal;
            _dbContext.Members.Update(user);
            var s1 = _dbContext.SaveChanges();

            var root = _dbContext.Members.FirstOrDefault(x => x.Account == "root");
            root.Balance -= returnVal;
            _dbContext.Members.Update(root);
            var s2 = _dbContext.SaveChanges();

            return s1 == 1 && s2 == 1;
        }

        public bool UpdateRootBalance(decimal newBalance)
        {
            var root = _dbContext.Members.FirstOrDefault(member => member.Account == "root");
            root.Balance = newBalance;
            _dbContext.Members.Update(root);
            var affectRows = _dbContext.SaveChanges();

            return affectRows == 1;
        }

        public bool UpdateUserBalance(Guid id, decimal newBalance)
        {
            var member = _dbContext.Members.FirstOrDefault(member => member.Id == id);
            member.Balance = newBalance;

            _dbContext.Members.Update(member);
            var affectRows = _dbContext.SaveChanges();

            return affectRows == 1;
        }
    }
}

