using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.Features.Accounts.Aggregate
{
    public class AccountCreated
    {
        public Guid Id { get; set; }

        public string Type { get; }

        public decimal Balance { get; }

        public AccountCreated(Guid id, string type, decimal balance)
        {
            Id = id;
            Type = type;
            Balance = balance;
        }
    }
}
