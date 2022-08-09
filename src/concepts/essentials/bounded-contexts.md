# Stadium example

Domain-driven design has the concept of a bounded context (BC).  Have a look at [how Martin Fowler defines this](http://martinfowler.com/bliki/BoundedContext.html).  In that definition he also refers to what he calls [multiple canonical models](http://martinfowler.com/bliki/MultipleCanonicalModels.html).

To make the concept of a bounded context more clear we could take seats in a stadium as an example.  A stadium contains seats but a seat represents different things to different people.

## Financial Management

In most scenarios one would have to keep an asset register and it is typically owned by the financial folks since they are the *system of record* for any assets.  

The finance department would be interested in **data** such as:

- asset number
- asset type
- date of purchase
- cost
- method of depreciation
- date commissioned
- date decommissioned

They would also be interested in the following **behaviour**:

- Commission()
- Depreciate()
- Decommission()

## Maintenance Management

The maintenance department would be interested in **data** such as:

- maintenance required
- maintenance schedules

The **behaviour** for them would be along the lines of:

- ScheduleMaintenance()
- RegisterMaintenanceSchedule()

## Event Management

The event booking folks would be have **data** like this:

- event name
- event date
- relevant seat number(s)

And the relevant **behaviour** would include:

- RegisterEvent()
- SellTicket()

## Putting it all together

So, as we can see, even though a physical seat is one thing it can mean very different things to different people.  This is where a *Bounded Context* comes in.  You may even find that a similar concept is called something else in each context.  You may find that an `Employee` in the *HR* BC is called a `User` in the *Identity & Access Control* BC, or an `Author` in the *Collaboration* BC. 

As behaviour is invoked on various objects within each BC the other BCs may need to be informed.  To accomplish this one would need some communication mechanism based on an *Event Driven Architecture*.  That is, of course, where a service bus like Shuttle.Esb would come in handy.

The *Financial Management* BC could publish an `AssetRegistered` and the other BCs would subscribe to that event and then determine if they need to register the asset number as an item they are interested in.

When the *Maintenance Management* BC removes a seat for maintenance it would publish a `ItemRemovedForMaintenance` that the *Booking Management* BC would subscribe to in order to exclude the seat as a bookable item for any events until the item is made available again.

## Canonical Models

All this is quite different and contrasting to what many enterprise-level architects refer to as a canonical data model.  Stefan Tilkov has [something to say](https://www.innoq.com/en/blog/thoughts-on-a-canonical-data-model/) about these too.

For our scenario a canonical model would need to include all the data from the various BCs.  This is quite cumbersome and really does not add any real value.

