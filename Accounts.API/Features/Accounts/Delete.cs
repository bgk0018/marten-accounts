using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.API.Features.Accounts
{
    public static class Delete
    {
        public class Request
        {
            [FromRoute(Name = "id")]
            public Guid Id { get; set;  }

            [FromHeader(Name = "X-Correlation-ID")]
            public Guid CorrelationId { get; set; }
        }
    }
}
