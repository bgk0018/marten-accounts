using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.Features.Accounts.Aggregate
{
    public class AccountCredited
    {
        public Guid Id { get; set; }
        public decimal Amount { get; }
        public string Description { get; }

        public AccountCredited(Guid id, decimal amount, string description)
        {
            Id = id;
            Amount = amount;
            Description = description;
        }
    }
}
