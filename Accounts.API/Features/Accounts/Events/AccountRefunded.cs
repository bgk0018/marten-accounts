using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.Features.Accounts.Aggregate
{
    public class AccountRefunded
    {
        public Guid Id { get; }
        public Guid Target { get; }
        public decimal Amount { get; }

        public AccountRefunded(Guid id, Guid target, decimal amount)
        {
            Id = id;
            Target = target;
            Amount = amount;
        }
    }
}
