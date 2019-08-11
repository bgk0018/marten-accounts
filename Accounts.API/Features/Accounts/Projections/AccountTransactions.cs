using Accounts.API.Features.Accounts.Aggregate;
using System;
using System.Collections.Generic;

namespace Accounts.API.Features.Accounts.Projections
{
    public class Transaction
    {
        public string Type { get; set; }

        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }
    }

    public class AccountTransactions
    {
        public Guid Id { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public AccountTransactions()
        {
            //Required for serialization in marten
        }

        public void Apply(AccountCreated @event)
        {
            Transactions.Add(new Transaction()
            {
                Amount = @event.Balance,
                Id = @event.Id,
                Type = "Credit",
                Description = "Account created"
            });
        }

        public void Apply(AccountDebited @event)
        {
            Transactions.Add(new Transaction()
            {
                Amount = @event.Amount,
                Id = @event.Id,
                Type = "Debit",
                Description = @event.Description
            });
        }

        public void Apply(AccountCredited @event)
        {
            Transactions.Add(new Transaction()
            {
                Amount = @event.Amount,
                Id = @event.Id,
                Type = "Credit",
                Description = @event.Description
            });
        }

        public void Apply(AccountRefunded @event)
        {
            Transactions.Add(new Transaction()
            {
                Amount = @event.Amount,
                Id = @event.Id,
                Type = "Credit",
                Description = $"Account refunded for transaction {@event.Target}"
            });
        }
    }
}
