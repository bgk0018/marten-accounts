using Accounts.API.Features.Accounts.Aggregate;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.Features.Accounts
{
    public static class Update
    {
        public sealed class Request
        {
            [FromBody]
            public JsonPatchDocument<Account> JsonPatchDocument { get; }

            [FromRoute(Name = "id")]
            public Guid Id { get; }

            [FromHeader(Name = "X-Correlation-ID")]
            public Guid CorrelationId { get; }
        }
    }
}
