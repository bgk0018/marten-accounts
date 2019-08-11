using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.Features.Accounts.Aggregate
{
    public class Account
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string State { get; set; }

        public decimal Balance { get; set; }

        public bool Deleted { get; set; }

        public Account()
        {
            //Required for serialization in marten
        }

        public void Apply(AccountCreated @event)
        {
            Id = @event.AccountId;
            Type = @event.Type;
            State = "Open";
            Balance = @event.Balance;
        }

        public void Apply(AccountDeleted @event)
        {
            Deleted = true;
        }

        public void Apply(AccountUpdated @event)
        {
            @event.Patch.ApplyTo(this);
        }

        public void Apply(AccountDebited @event)
        {
            if(@event.Amount > Balance && State == "Open")
            {
                State = "Frozen";
            }

            Balance -= @event.Amount;
        }

        public void Apply(AccountCredited @event)
        {
            Balance += @event.Amount;

            if (Balance > 0 && State == "Frozen")
            {
                State = "Open";
            }
        }
    }
}
