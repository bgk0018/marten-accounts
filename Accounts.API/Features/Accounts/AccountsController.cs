using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounts.API.Features.Accounts;
using Accounts.API.Features.Accounts.Aggregate;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IDocumentSession session;

        public AccountsController(IDocumentSession session)
        {
            this.session = session;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(Create.Request request)
        {
            if(request.Model == null)
            {
                return BadRequest("Invalid body.");
            }

            var @event = new AccountCreated(
                request.CorrelationId, 
                request.Model.Id, 
                request.Model.Type, 
                request.Model.Balance);

            session.Events.StartStream<Account>(request.Model.Id, @event);

            await session.SaveChangesAsync(new CancellationToken()).ConfigureAwait(false);

            return StatusCode(201); //return the route you dingus.
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Delete.Request request)
        {
            var account = await session
                    .Query<Account>()
                    .Where(p => !p.Deleted)
                    .Where(p => p.Id == request.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

            if (account == null)
            {
                return NotFound();
            }

            session.Events.Append(request.Id, new AccountDeleted(request.CorrelationId));
            await session.SaveChangesAsync(new CancellationToken());

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await session.Query<Account>().Where(p => p.Deleted != true).ToListAsync().ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.NewGuid())
            {
                return BadRequest("Bad id");
            }

            var result = await session
                .Query<Account>()
                .Where(p => p.Id == id)
                .Where(p => p.Deleted != true)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPatch, Route("{id:guid}")]
        public async Task<IActionResult> Patch(Update.Request request)
        {
            var token = new CancellationToken();

            var cube = await session.Events
                    .AggregateStreamAsync<Account>(request.Id, token: token)
                    .ConfigureAwait(false);

            if (cube?.Deleted != false)
                return NotFound();

            session.Events.Append(request.Id, new AccountUpdated(request.CorrelationId, request.JsonPatchDocument));
            await session.SaveChangesAsync(token).ConfigureAwait(false);

            return Ok();
        }
    }
}
