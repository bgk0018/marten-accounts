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
    [Route("[controller]")]
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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Delete.Request request)
        {
            var account = await session
                    .Query<Account>()
                    .Where(p => p.Deleted != true)
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

            var cube = await session
                .Query<Account>()
                .Where(p=>p.Id == request.Id)
                .Where(p => p.Deleted != true)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (cube == null)
                return NotFound();

            session.Events.Append(request.Id, new AccountUpdated(request.CorrelationId, request.JsonPatchDocument));
            await session.SaveChangesAsync(token).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost("{id:guid}/debits")]
        public async Task<IActionResult> PostDebit(Debit.Request request)
        {
            if (request.Model == null)
            {
                return BadRequest("Invalid body.");
            }

            var result = await session
                .Query<Account>()
                .Where(p => p.Id == request.Id)
                .Where(p => p.Deleted != true)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (result == null)
            {
                return NotFound();
            }

            if(result.State == "Frozen")
            {
                return BadRequest("Account is frozen.");
            }

            session.Events.Append(request.Id, new AccountDebited(request.CorrelationId, request.Model.Amount));
            await session.SaveChangesAsync(new CancellationToken()).ConfigureAwait(false);

            return StatusCode(201); //return the route you dingus.
        }

        [HttpPost("{id:guid}/credits")]
        public async Task<IActionResult> PostCredit(Credit.Request request)
        {
            if (request.Model == null)
            {
                return BadRequest("Invalid body.");
            }

            var result = await session
                .Query<Account>()
                .Where(p => p.Id == request.Id)
                .Where(p => p.Deleted != true)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (result == null)
            {
                return NotFound();
            }

            session.Events.Append(request.Id, new AccountCredited(request.CorrelationId, request.Model.Amount));
            await session.SaveChangesAsync(new CancellationToken()).ConfigureAwait(false);

            return StatusCode(201); //return the route you dingus.
        }
    }
}
