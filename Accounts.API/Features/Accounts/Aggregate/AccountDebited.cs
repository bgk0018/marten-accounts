﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.Features.Accounts.Aggregate
{
    public class AccountDebited
    {

        public Guid Id { get; }
        public decimal Amount { get; }

        public AccountDebited(Guid id, decimal amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}
