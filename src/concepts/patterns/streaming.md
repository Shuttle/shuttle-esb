# Streaming

With message streaming events are produced in a continuous stream that isn't targeted at any consumers.  The streams are typically divided in topics and consumers can read messages from a topic.  Consumers are typically identified as a logical unit using some form of identifier.  Topcis may also be partitioned and one would have at most the number of consumers as there are partitions.

The number of messages kept in the stream is determined by a retention period and consumers may start processing messahes from any point in the stream; although typically either the oldest/earliest/tail or most recent/newest/head would be used as a starting point.  Messages are, therefore, not removed once consumed but are rather removed once they reach the retention period; although sometimes a maximum size for the stream is also used.  This is in contrast to a queue where a message is target at a particular logical endpoint and once processed the message is removed.

![Streaming Image](/images/streaming.png)