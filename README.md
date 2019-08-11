Events
* Events come in many flavors that can be pushed onto a stream
* Events pushed onto a stream are responsible for modeling the state of the stream


On Creation:
* An Aggregate type is just used as a marker type for that stream
* Creation just requires a new event and a new stream id (guid)

* Projections are read side


* Marten can also be used as just a flat document store
