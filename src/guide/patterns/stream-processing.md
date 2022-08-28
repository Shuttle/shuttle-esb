# Stream Processing

::: info
Remember that you can download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.
:::

This sample makes use of [Kafka](https://kafka.apache.org/) for the streams.

If you use [Docker](https://www.docker.com/) you can get up-and-running with the following `docker-compose.yml` file:

```yaml
---
version: '2'
services:
  zookeeper:
    image: confluentinc/cp-zookeeper:7.1.1
    hostname: zookeeper
    container_name: zookeeper
    ports:
      - "2181:2181"
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
      ZOOKEEPER_TICK_TIME: 2000

  broker:
    image: confluentinc/cp-server:7.1.1
    hostname: broker
    container_name: broker
    depends_on:
      - zookeeper
    ports:
      - "9092:9092"
      - "9101:9101"
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: 'zookeeper:2181'
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://broker:29092,PLAINTEXT_HOST://localhost:9092
      KAFKA_METRIC_REPORTERS: io.confluent.metrics.reporter.ConfluentMetricsReporter
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
      KAFKA_GROUP_INITIAL_REBALANCE_DELAY_MS: 0
      KAFKA_CONFLUENT_LICENSE_TOPIC_REPLICATION_FACTOR: 1
      KAFKA_CONFLUENT_BALANCER_TOPIC_REPLICATION_FACTOR: 1
      KAFKA_TRANSACTION_STATE_LOG_MIN_ISR: 1
      KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR: 1
      KAFKA_JMX_PORT: 9101
      KAFKA_JMX_HOSTNAME: localhost
      KAFKA_CONFLUENT_SCHEMA_REGISTRY_URL: http://schema-registry:8081
      CONFLUENT_METRICS_REPORTER_BOOTSTRAP_SERVERS: broker:29092
      CONFLUENT_METRICS_REPORTER_TOPIC_REPLICAS: 1
      CONFLUENT_METRICS_ENABLE: 'true'
      CONFLUENT_SUPPORT_CUSTOMER_ID: 'anonymous'

  kafka-ui:
    container_name: kafka-ui
    image: provectuslabs/kafka-ui:latest
    ports:
      - 8080:8080
    depends_on:
      - zookeeper
      - broker
    environment:
      KAFKA_CLUSTERS_0_NAME: local
      KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS: broker:29092
      KAFKA_CLUSTERS_0_ZOOKEEPER: zookeeper0:2181
```

Once you have opened the `Shuttle.StreamProcessing.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.StreamProcessing.Producer
- Shuttle.StreamProcessing.Consumer

## Implementation

In order to get any processing done in Shuttle.Esb a message will need to be produced and sent to a stream, usually represented by a topic, for processing.

In this guide we'll create the following projects:

- `Shuttle.StreamProcessing.Producer` (**Console Application**)
- `Shuttle.StreamProcessing.Consumer` (**Console Application**)
- `Shuttle.StreamProcessing.Messages` (**Class Library**)

## Messages

> Create a new class library called `Shuttle.StreamProcessing.Messages` with a solution called `Shuttle.StreamProcessing`

**Note**: remember to change the *Solution name*.

### TemperatureRead

> Rename the `Class1` default file to `TemperatureRead` and add a `Name`, `Minute` and 'Celsius' property.

``` c#
namespace Shuttle.StreamProcessing.Messages
{
	public class TemperatureRead
	{
		public string Name { get; set; }
		public int Minute { get; set; }
		public decimal Celsius { get; set; }
	}
}
```

## Producer

> Add a new `Console Application` to the solution called `Shuttle.StreamProcessing.Producer`.

> Install the `Shuttle.Esb.Kafka` nuget package.

This will provide access to the Kafka `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

> Add a reference to the `Shuttle.StreamProcessing.Messages` project.

### Program

> Implement the producer code as follows:

```c#
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Esb;
using Shuttle.Esb.Kafka;
using Shuttle.StreamProcessing.Messages;

namespace Shuttle.StreamProcessing.Producer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddServiceBus(builder =>
            {
                configuration.GetSection(ServiceBusOptions.SectionName).Bind(builder.Options);
            });

            services.AddKafka(builder =>
            {
                builder.AddOptions("local", new KafkaOptions
                {
                    BootstrapServers = "localhost:9092",
                    EnableAutoCommit = true,
                    EnableAutoOffsetStore = true,
                    NumPartitions = 1,
                    UseCancellationToken = false,
                    ConsumeTimeout = TimeSpan.FromMilliseconds(25)
                });
            });

            Console.WriteLine("Type a name for the set of readings, then press [enter] to submit; an empty line submission stops execution:");
            Console.WriteLine();

            var random = new Random();
            decimal temperature = random.Next(-5, 30);

            using (var bus = services.BuildServiceProvider().GetRequiredService<IServiceBus>().Start())
            {
                string name;

                while (!string.IsNullOrEmpty(name = Console.ReadLine()))
                {
                    for (var minute = 0; minute < 1440; minute++)
                    {
                        bus.Send(new TemperatureRead
                        {
                            Name = name,
                            Minute = minute,
                            Celsius = temperature
                        });

                        if (temperature > -5 && (random.Next(0, 100) < 50 || temperature > 29))
                        {
                            temperature -= random.Next(0, 100) / 100m;
                        }
                        else
                        {
                            temperature += random.Next(0, 100) / 100m;
                        }
                    }
                }
            }
        }
    }
}
```

### Producer configuration file

> Add an `appsettings.json` file as follows:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "MessageRoutes": [
        {
          "Uri": "kafka://local/stream-consumer-work",
          "Specifications": [
            {
              "Name": "StartsWith",
              "Value": "Shuttle.StreamProcessing.Messages"
            }
          ]
        }
      ]
    } 
  } 
}
```

This tells the service bus that all messages sent having a type name starting with `Shuttle.StreamProcessing.Messages` should be routed to endpoint `kafka://local/stream-consumer-work`.

## Consumer

> Add a new `Console Application` to the solution called `Shuttle.StreamProcessing.Consumer`.

> Install the `Shuttle.Esb.Kafka` nuget package.

This will provide access to the Kafka `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Hosting` nuget package.

This allows a console application to be hosted using the .NET generic host.

> Install the `Microsoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

> Add a reference to the `Shuttle.StreamProcessing.Messages` project.

### Program

> Implement the `Program` class as follows:

``` c#
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.Esb;
using Shuttle.Esb.Kafka;

namespace Shuttle.StreamProcessing.Consumer
{
    internal class Program
    {
        private static void Main()
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                    services.AddSingleton<IConfiguration>(configuration);

                    services.AddServiceBus(builder =>
                    {
                        configuration.GetSection(ServiceBusOptions.SectionName).Bind(builder.Options);
                    });

                    services.AddKafka(builder =>
                    {
                        builder.AddOptions("local", new KafkaOptions
                        {
                            BootstrapServers = "localhost:9092",
                            EnableAutoCommit = true,
                            EnableAutoOffsetStore = true,
                            NumPartitions = 1,
                            UseCancellationToken = false,
                            ConsumeTimeout = TimeSpan.FromMilliseconds(25)
                        });
                    });
                })
                .Build()
                .Run();
        }
    }
}
```

### Consumer configuration file

> Add an `appsettings.json` file as follows:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "Inbox": {
        "WorkQueueUri": "kafka://local/stream-consumer-work"
      }
    }
  }
}
```

### TemperatureReadHandler

> Add a new class called `TemperatureReadHandler` that implements the `IMessageHandler<TemperatureReadHandler>` interface as follows:

``` c#
using Shuttle.Esb;
using Shuttle.StreamProcessing.Messages;

namespace Shuttle.StreamProcessing.Consumer;

public class TemperatureReadHandler : IMessageHandler<TemperatureRead>
{
    public void ProcessMessage(IHandlerContext<TemperatureRead> context)
    {
        Console.WriteLine($"[TEMPERATURE READ] : name = '{context.Message.Name}' / minute = {context.Message.Minute} / celsius = {context.Message.Celsius:F}");
    }
}
```

## Run

> Set both the producer and consumer projects as startup projects.

### Execute

In order to successfully execute the solution you would need a locally accessible Kafka instance.

> Execute the application.

> The **producer** application will wait for you to input a reading set name.  For this example enter today's date and press enter:

::: info 
You will observe that the consumer application has processed the messages.
:::

You have now completed a full stream processing implementation.

