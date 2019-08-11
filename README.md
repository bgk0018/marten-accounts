# Marten Event Sourcing HTTP API
This is a toy API for understanding how you can pull together Marten as an Event Store within a .NET Core API. This emulates the ability of a bank accounting system.

Some of the things touched on are:

* Fetching Streams (Refund)
* Aggregates (Account)
* Live Projections (AccountTransactions)
* Inline Projections (Account)
* Daemon/Async Projections (?)

## Accounts

Events
* Events come in many flavors that can be pushed onto a stream
* Events pushed onto a stream are responsible for modeling the state of the stream


On Creation:
* An Aggregate type is just used as a marker type for that stream
* Creation just requires a new event and a new stream id (guid)

* Projections are read side


* Marten can also be used as just a flat document store

## Libraries

A few libraries were used for sanity reasons:

* Opinionated/Required CorrselationId: CorrelationIdMiddleware (Depends on FluentValidation)
* Easy Global Exception Handling: GlobalExceptionHandler
* Feature Folders: OdeToCode.AddFeatureFolders
* Documentation: Swashbuckle.AspNetCore
