using Microsoft.AspNetCore.Mvc;
using System;

namespace Accounts.API.Features.Accounts
{
    public static class Create
    {
        public class Request
        {
            [FromBody]
            public Model Model { get; set; }

            [FromHeader(Name = "X-Correlation-ID")]
            public Guid CorrelationId { get; set; }
        }

        public class Model
        {
            public Guid Id { get; set; }

            public string Type { get; set; }

            public decimal Balance { get; set; }
        }
    }
}
