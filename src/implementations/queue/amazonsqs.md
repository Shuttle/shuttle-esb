# Amazon SQS

```
PM> Install-Package Shuttle.Esb.AmazonSqs
```

In order to make use of the `AmazonSqsQueue` you will need access to an [Amazon Web Services](https://aws.amazon.com/sqs/) account.  There are some options for local development which are beyond the scope of this documentation.

You may want to take a look at [Messaging Using Amazon SQS](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/sqs-apis-intro.html).

## Configuration

The queue configuration is part of the specified uri, e.g.:

``` xml
<inbox
    workQueueUri="amazonsqs://endpoint-name/queue-name?maxMessages=15&amp;waitTimeSeconds=20"
    .
    .
    .
/>
```

| Segment / Argument | Default | Description |
| --- | --- | --- | 
| endpoint-name | required | Will be resolved by an `IAmazonSqsConfiguration` implementation (*see below*). |
| queue-name | required | The name of queue to connect to. |
| maxMessages | 1 | Specifies the number of messages to fetch from the queue. |
| waitTimeSeconds | 20 | Specifies the number of seconds to perform the long-polling. |

## IAmazonSqsConfiguration

```c#
AmazonSQSConfig GetConfiguration(string endpointName);
```

The `GetConfiguration()` method should return the [Amazon.SQS.AmazonSQSConfig](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SQS/TSQSConfig.html) instance that will be used by the [AmazonSQSClient](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SQS/TSQSClient.html) to interact with the relevant Amazon SQS queue.

The relevant `IAmazonSqsConfiguration` should be registered with the `IComponentRegistry`:

```c#
IComponentResolver.Register<IAmazonSqsConfiguration, DefaultAmazonSqsConfiguration>;
```

## DefaultAmazonSqsConfiguration

This implementation will add all the endpoints provided in the application configuration file:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <section name="amazonsqs" type="Shuttle.Esb.AmazonSqs.AmazonSqsSection, Shuttle.Esb.AmazonSqs"/>
    </configSections>

    <amazonsqs>
        <endpoints>
            <endpoint name="endpoint-a" serviceUrl="https://sqs.us-east-1.amazonaws.com/123456789012" />
            <endpoint name="endpoint-b" serviceUrl="https://sqs.us-east-2.amazonaws.com/123456789012" />
        </endpoints>
    </amazonsqs>
</configuration>

<inbox
    workQueueUri="amazonsqs://endpoint-a/server-inbox-work-queue"
    .
    .
    .
/>
```
