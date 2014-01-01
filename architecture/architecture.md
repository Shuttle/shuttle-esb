# Fundamental Concepts

Code samples provided on this page do not represent a sample or solution but do show how some of the concepts would be applied in Shuttle ESB.  For help on putting together your first implementation you can take a look at the [getting started](/getting-started/index.html) page.

The basic parts of Shuttle ESB consist of:

* Messages
* Queues
* Service bus

Every service bus instance is associated, and therefore processes, only one queue.  This is the inbox.  All messages received in the inbox are processed by the associated service bus instance.