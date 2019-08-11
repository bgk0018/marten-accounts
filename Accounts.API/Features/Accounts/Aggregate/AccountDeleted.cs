using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.Features.Accounts.Aggregate
{
    public class AccountDeleted
    {
        public Guid Id { get; }

        public AccountDeleted(Guid id)
        {
            Id = id;
        }
    }
}
