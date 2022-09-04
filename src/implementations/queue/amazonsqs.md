# Amazon SQS

```
PM> Install-Package Shuttle.Esb.AmazonSqs
```

In order to make use of the `AmazonSqsQueue` you will need access to an [Amazon Web Services](https://aws.amazon.com/sqs/) account.  There are some options for local development, such as [ElasticMQ](https://github.com/softwaremill/elasticmq), which are beyond the scope of this documentation.

You may also want to take a look at [Messaging Using Amazon SQS](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/sqs-apis-intro.html).

## Configuration

The URI structure is `amazonsqs://configuration-name/queue-name`.

```c#
services.AddAmazonSqs(builder =>
{
    var amazonSqsOptions = new AmazonSqsOptions
    {
        ServiceUrl = "http://localhost:9324",
        MaxMessages = 1,
        WaitTime = TimeSpan.FromSeconds(20)
    };

    amazonSqsOptions.Configure += (sender, args) =>
    {
        Console.WriteLine($"[event] : Configure / Uri = '{((IQueue)sender).Uri}'");
    };

    builder.AddOptions("local", amazonSqsOptions);
});
```

The `Configure` event `args` arugment exposes the `AmazonSQSConfig` directly for any specific options that need to be set.

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "AmazonSqs": {
      "local": {
        "ServiceUrl": "http://localhost:9324",
        "MaxMessages": 5,
        "WaitTime": "00:00:20"
      },
      "proper": {
        "ServiceUrl": "https://sqs.us-east-2.amazonaws.com/123456789012/MyQueue"
      }
    }
  }
}
```

## Options

| Option | Default | Description |
| --- | --- | --- | 
| ServiceUrl |  | The URL to connect to. |
| MaxMessages | 10 | Specifies the number of messages to fetch from the queue. |
| WaitTime | 00:00:20 | Specifies the `TimeSpan` duration to perform long-polling. |
