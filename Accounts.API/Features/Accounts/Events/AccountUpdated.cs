using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace Accounts.API.Features.Accounts.Aggregate
{
    public class AccountUpdated
    {
        public Guid Id { get; set; }
        public JsonPatchDocument<Account> Patch { get; }

        public AccountUpdated(Guid id, JsonPatchDocument<Account> patch)
        {
            Id = id;
            Patch = patch;
        }
    }
}
