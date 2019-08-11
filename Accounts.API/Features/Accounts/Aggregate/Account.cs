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
    }
}
