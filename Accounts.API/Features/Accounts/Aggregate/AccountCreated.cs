using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.Features.Accounts.Aggregate
{
    public class AccountCreated
    {
        public Guid Id { get; }

        public Guid AccountId { get; }

        public string Type { get; }

        public decimal Balance { get; }

        public AccountCreated(Guid id, Guid accountId, string type, decimal balance)
        {
            Id = id;
            AccountId = accountId;
            Type = type;
            Balance = balance;
        }
    }
}
