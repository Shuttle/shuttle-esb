---
title: MoonSpring Discussions
layout: post
published: false
---

# the Server return message to the Client 

### MoonSpring (Jul 30 at 7:22 AM)
	
Hi,I meet a problem about shuttle.I download a demo about it,It only has one function:The Client can send message to Server,and the Server can print the message in Console.
Now I have to change it. I hope the Server can return message to the Client.
I want to ask:Does it work? And what can I do?

And then shuttle is new to me. I can't find a API about,can you help me? Thanks very much.I hope your reply.
	
### jabberwocky (Jul 30 at 7:47 AM)
	
Hello,

The Shuttle.Esb project has moved over to github: https://github.com/Shuttle/Shuttle.Esb

The documentation is here: http://shuttle.github.io/Shuttle.Esb/

There are also some videos (have a look at the v3 ones): https://www.youtube.com/user/shuttleservicebus

The Request/Response sample (https://github.com/Shuttle/Shuttle.Esb.Samples) does demonstrate the pattern that you are referring to.

Should you experience any issues or have question please do not hesitate to log an issue on GitHub under the relevant repository.

Regards,
Eben
	
### MoonSpring (Jul 31 at 4:33 AM)
	
Thanks very much for your reply.This does help me a lot.I will start my journey about Shuttle.Esb.It's a good start.Thank you again.

---

# hope you give me a little hint about my demand 

### MoonSpring (Aug 6 at 3:30 AM)
	
Now I want to realize this function:
Now We have ClientA,ClientB,clientC,and ServerD.
I need ClientA send a message to ServerD(Just like the first demo:QuickStart.Shuttle),
and then I need ServerD send this message to ClientB and ClientC(Just like the demo:Pub/Sub).

So I have to put this two demos together.Now I have no thought about how to do this.Can you give me a little hint?
Thanks,my friends.
	
### jabberwocky (Aug 6 at 6:00 AM)
	
Hello,

You use 'send' when working with commands. They go to 1 endpoint and they must have an endpoint.

You use 'publish' when something interesting has happened that other endpoints may be interested in. There can be 0 to N subscribers.

In your scenario I would 'send' a command from ClientA to ServerD. Then ClientB and ClientC need to subscribe to an event that you 'publish' from ServerD after the processing.

Of course ClientB and ClientC will need handlers for the given event.

In the pub/sub video (https://www.youtube.com/watch?v=_q7yu_9ZVo0) this scenario is illustrated, but instead of the client subscribing one would have your other two clients subscribing.


Regards,
Eben
	
### MoonSpring (Aug 6 at 11:19 AM)
	
Hello,Today I've tryed it to put these two demos together.But it still has some bugs.
Such as:

> No treatment MissingMethodException
> Not found Method:"System.String Shuttle.Core.Infrastructure.ExecptionExtensions.AllMessages(System.Exception)"

I've already Checked my codes for many times,but I still can't solve the problem.
According to this error message,I guess maybe it's the Version's problem.
do you think so?
	
### jabberwocky (Aug 6 at 3:17 PM)
	
Hi,

Yes, it is definitely a version problem. The shuttle-core was updated and it now contains the AllMessages() extension method. So it seems as though you have newer Shuttle.Esb with older shuttle-core.

Perhaps try to a nuget "update-package".

Regards,
Eben
	
### MoonSpring (Aug 7 at 4:11 AM)
	
Hello,

I've already download the newest source code,but it still has the same problem.Through the internet,some guy said to me "this is because the problem of .NET FrameWork's version".My develop environment is .NET FrameWork4 ,and the source code is .NET FrameWork 3.5,Does it even matter a lot?

thank you for your help all the time.
	
### jabberwocky (Aug 7 at 6:13 AM)
	
Hi,

What source code are you referring to?

In your own code you will be using the binaries. The nuget packages have binaries for 3.5, 4.0, and 4.5. It isn't a framework version issue but rather an issue between the versions of shuttle-core and Shuttle.Esb.

Have you tried the nuget 'update-package' command? Well, that will only work if you have included your references from nuget.

Regards,
Eben
	
### MoonSpring (Aug 8 at 5:09 AM)
	
Hi,

Thanks you for your help my friends.It's OK now.But there are still some problems.
I hope you can help me again.Such as the following quesions.

1、It sometimes send this wrong message.

> No queue factory has been registered for scheme 'msmq'.

2、Do you remebmer the ClientA,ClientB,clientC,and ServerD demo that I told you before?I still can't realize it.My thought just like that:

> ClientA send a message to ServerD(Just like the first demo:QuickStart.Shuttle),
> Then ClientB and ClientC  subscribe to an event that you 'publish' from ServerD after the processing(pub/sub Demo).
> I want to put QuickStart.Shuttle.Server(from demo QuickStart.Shuttle) and PublishSubscribe.Client(from Demo pub/sub)  together.But I didn't know how to do it.Could you tell me a easy way to do this?

3、sorry,I still have the third question.

> Now I find the Subscriber can send message to Publicer in the pub/sub demo.So I have a thought to realize my function.I can use one of the Subscriber send message to the Publicer,and then the Publicer send it to others.But I really don't think that is a good idea.What do you think?

### jabberwocky (Aug 8 at 6:22 AM)
	
Hello,

When a service bus instance starts up the QueueManager finds all queue factories. Each factory is associated with a uri scheme. So when you get this message it means that shuttle has not found your msmq queue factory implementation. Ensure that it is in the binary folder.

What you describe in point 3 is exactly how you should solve your point 2 problem. When you have multiple messages involved in performing some process you'll find that your command endpoint typically raise events or send more commands to get the job done. For instance, let's say you register an order by sending a RegisterOrderCommand message. When that is processed on the Order.Server endpoint it may raise event OrderRegisteredEvent. In addition to this you need to, say, send an e-mail. To do this you send a command called SendEMailCommand to the EMail.Server endpoint. So not only is it perfectly valid but you will find yourself doing this all the time. When working with arbitrary sample names the conepts may not be quite as apparent :)

Regards,
Eben

---

# Pass the Parameters 

### MoonSpring (Aug 14 at 11:38 AM)
	
Hi,how are you?

I have use the shuttle to my project.
Now I met some problem.I want to ask a Entity thing.
In the demo,we use OrderCompletedEvent and WorkDoneEvent.When I make a little change,there are some problem.

First ,I want to know:in class file OrderCompletedEvent.cs,the meaning of the OrderId.
Do it relate to Serialization thing?
When I delete this field,It can still run and have no any wrong messages.

~~~ c#
namespace PublishSubscribe.Messages
{
	public class OrderCompletedEvent 
	{
		public Guid OrderId { get; set; }
		public String coment { get; set; }
	}
}
~~~

Second,I want to pass "List" type in my project instead of Entity,But it will send me the wrong message about: Serialization
	
### jabberwocky (Aug 14 at 1:33 PM)
	
Hi,

The first thing to remember is that the messages should not be entities or any domain object, for that matter. It is always a simple DTO (data transfer object). It should contain the relevant data that you need and the relevant entity ids.

Next you need to remember to be careful with message versions. Changing existing message structures may result in failures. If you do want to change a structure use another name or namespace. A namespace should be easier. If you are in development and versioning is not yet an issue (as it would be in production) then you could make a breaking change but you would need to remember to purge all message from the relevant queues.

Shuttle does not do anything special w.r.t. serialization so if it breaks it means that the structure is not compatible.

If you want to consider a namespace mechanism you could go with something like this:

~~~ c#
namespace Company.BC.Messages.V1
~~~

As soon as you encounter breaking changes you could have that message under:

~~~ c#
namespace Company.BC.Messages.V1
~~~

However, things get rather tricky when it comes to subscriptions and the like so these things need to be carefully considered.

Regards,
Eben

---

# Failed to load the file or assembly“Shuttle.Esb,Version=3.2.3.0,Culture=neutral,PublicKeyToken=null” 

### MoonSpring (Aug 22 at 1:28 PM)
	
Hi,
I have met a exception,like:

> Abnormal untreated:System.TypeInitiallizationException:"ICT.Server.Program" ,The type initializer was an exception.  ------->System.BadImageFormatExeption:Failed to load the file or assembly“Shuttle.Esb,Version=3.2.3.0,Culture=neutral,PublicKeyToken=null” orOne of its dependencies, trying to load is not in the correct format.

I think this is a version's problem about the .NET Framework.

I created there new project(According to the Pub/Sub demo),and then ,I added the Shuttle.Esb's source code I need.
I have already changed them to ".NET Framework 4.0"(Because I find all the source code projects are .NET Framework 3.5).
When I Compile all the code,It succeed.

But When I run it ,it will send me this wrong message.

the reason I do this,is because I want to use it in our Project.I used the demo I download,it can run normally.
However,When I create a new project,I met the problem above.

I guess it is the creating environment's problem.Do you think so?
	
### jabberwocky (Aug 22 at 8:02 PM)
	
Hi,

I would, of course, suggest that you rather use Nuget to get the binaries.

I you would like to build them yourself from the code you need to navigate to {base}\Shuttle.Esb\build\4.0 folder in a VS2012 command window and execute 'msbuild build.msbuild'. The {base}\Shuttle.Esb\build\ folder will contain a 'debug' and 'release' folder that will have the various framework version binaries.

Regards,
Eben
	
### MoonSpring (Aug 23 at 6:01 AM)
	
Hi,

Thank you very much,I've already used the Nuget tool,it can work normally.


Regards,
Spring.

---

# Can I create a Handler for a WPF winform? 

### MoonSpring (Aug 23 at 5:59 AM)
	
Hi,

I want to create a handler for the WPF winform, instead of the Console Application(I mean in the Sub point).

does it work normally?
I tryed,but i throw a strange wrong message:

> Component"MainMenu" is not recognized by the URI resource "mainmenu.xaml".

some online friends told me:this is because I rename my project's problem.But it can work normally without using the ESB parts.
	
### jabberwocky (Aug 23 at 7:50 AM)
	
Hello,

Shuttle should not interfere with any particular platform as it is simply a library. However, seeing as it is software, anything is possible :)

I cannot think off-hand what could cause this. Perhaps you can send me a simplified version that demonstrates the problem and I'll take a look.

My e-mail address: me@ebenroux.co.za

Regards,
Eben
	
### MoonSpring (Aug 23 at 8:23 AM)
	
Hi,

I've already know the reason about the wrong message.

I create a WPF winform project named SubA,and then I changed the way to start as the "Shuttle.Core.Host.exe".
But because the winform will Compile to "SubA.exe" file. If I start it as the "Shuttle.Core.Host.exe",it can't start as the "SubA.exe" file.

Now I've already tryed.I can use the WPF winform.And it can run normally.

But in my project.It still have the same problem:

> Component"MainMenu" is not recognized by the URI resource "mainmenu.xaml". 

In my project,I want to use the "MainFormwork" part as the Sub port.
In the "MainFormWork", there are some Dynamic menu,each menu is a Display terminal.Do it affect?
	
### MoonSpring (Aug 25 at 8:59 AM)
	
Hi,

I've already find the problem.My project used some third-party products,when I add the Shuttle in,it will have the problem.
This problem is not a big deal,I'll debug it.
Thanks.

---

# My business is suitable for using the Shuttle or not

### MoonSpring (Aug 21 at 11:33 AM)
	
Hi,
sorry to bother you again.

Please give me a little hint about how to use the Shuttle.

Let's see a demo first:
Client A ,Server A, Client1,Client2,Client 3

I need that:If ever ClientA 's message changed,ClientA need to send a message to Server A, then the ServerA can send the message to Client1 ,Client2 and Client3.

If ever Client A make sure it received the message,then it will told Client2 and Client 3 than it has already changed.
And so do Client2 and Client3.

Here is my thought:
ServerA can send a message ,then Client1,Client2 and Client3 can receive the message.But how can I let Client1 let the other two clients know it, when the Client1 changed.
	
### jabberwocky (Aug 21 at 2:19 PM)
	
Hello,

Whenever you want an arbitrary number of endpoints (1 or more) to be able to pick up on changes it would probably be an event that you need to publish.

Perhaps if you think about clients and servers more in terms of producers and consumers, or even publishers and subscribers, things may make more sense.

Since you are not using anything concrete in your example I'll try to answer as best I can and I'll use your names.

Client A sends a command to Server A for processing. Let's call this StartEngineCommand.

Server A handles the StartEngineCommand and publishes the EngineStartedEvent.

Client 1, Client 2, and Client 3 subcribe to event EngineStartedEvent so they can handle it.

Now let's assume Client 1 does something like changing the engine's RPM to 2000. Client 1 could publish event EngineRPMChangedEvent.

Client 2 and Client 3 could subscribe to EngineRPMChangedEvent and handle the event by implementing some relevant functionality.

I hope that helps.

Shuttle, like any service bus, will be highly useful (and valuable) in a distributed, decoupled, cross-process communication architecture.

Regards,
Eben
	
### MoonSpring (Aug 25 at 8:12 AM)
	
Hi,

As you said,I've already know your meaning.
But I don't know how to deal with the "App.config",and I don't know how to write the serviceBus configuration.Such as the Client2.

Server A publishes the EngineStartedEvent.
Client 1, Client 2, and Client 3 subcribe to event EngineStartedEvent so they can handle it.

What if Client2 changed,or Client2 and Client3 changed at the same time,what should I do in the "App.config"?

And should I create a new ServerBus?
	
### jabberwocky (Aug 25 at 10:38 AM)
	
Hello,

Each endpoint must have its own service bus instance. Each must have its own inbox queue. So each one will have its own application configuration file.

The application configuration (by default) is used to define the inbox and the message routing. The subscriptions, however, require an implementation instance of the ISubscriptionManager interface and, typically, the subscribers will all point to the same subscription store. The subscribers will need to subscribe to the relevant events either at startup (at least to test that the subscription exists) or manually using the management shell.

I don't quite understand what you mean by the endpoints changing at the same time.

Regards,
Eben

---

# Component“ICT.RCS.Modules.Client.MainMenu.MainMenu” is not recognized by the URI resource“/ICT.RCS.Modules.Client.MainMenu;component/mainmenu.xaml” 

### MoonSpring (Aug 26 at 11:35 AM)
	
Hi,

Do you still remember this problem? I asked you about it a few days ago.

> Component“ICT.RCS.Modules.Client.MainMenu.MainMenu” is not recognized by the URI resource“/ICT.RCS.Modules.Client.MainMenu;component/mainmenu.xaml”

I would like to send you a simple demo,but the demo alway run normally and I don't readlly know the reasons.However,when I added the Shuttle into my project,it will still have the problem.

I spent the whole day to find this reason.Now I will try to express it clearly.

ICT.WPF.Client.MainFramework( the Subscriber port ),is one name of my project.It will work with the WCF and arcgis Map Service,so the "App.config" will be like this:

~~~ xml
 <?xml version="1.0" encoding="utf-8" ?>
  <configuration>
   <configSections>
 <section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
  </configSections>
  <connectionStrings>
 <clear/>
 <add name="Subscription" connectionString="Uid=sa;Pwd=123456;Initial Catalog=shuttle;Data Source=172.22.51.119;Connect Timeout=900" providerName="System.Data.SqlClient"/>
  </connectionStrings>
  <serviceBus>
 <inbox
  workQueueUri="msmq://./pubsub-MainClient-inbox-work"
  errorQueueUri="msmq://./shuttle-pubsub-error"/>
</serviceBus>
<startup>
  <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.0"/>
</startup>
<runtime>
 <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
  <probing privatePath="bin;..\..\Release\Program.MainFramework\Services;..\..\Release\Program.MainFramework\Modules;..\..\Release\Program.MainFramework\Interfaces"/>
 </assemblyBinding>
</runtime>
<appSettings>
<!--arcgis Map -->
<add key="alarmArcServerUrl" value="http://wxy-f9cic5ku08d:8399/arcgis/services" />
<!--arcgis Map ServiceName -->
<add key="alarmArcServerName" value="bj140814" />
<add key="istestMode" value="true"/>
<add key="ClientSettingsProvider.ServiceUri" value="" />
  </appSettings>

  <system.serviceModel>
  <behaviors>
  <endpointBehaviors>
    <behavior name="bhc">
      <dataContractSerializer maxItemsInObjectGraph="2147483647"/>
    </behavior>
    </endpointBehaviors>
  </behaviors>
  <bindings>
    <wsHttpBinding>
    <binding name="WSHttpBinding_WcfService" closeTimeout="00:01:00"
         openTimeout="00:01:00" receiveTimeout="00:10:00" sendTimeout="00:10:00"
         bypassProxyOnLocal="false" transactionFlow="false" hostNameComparisonMode="StrongWildcard"
         maxBufferPoolSize="2147483647" maxReceivedMessageSize="2147483647"
         messageEncoding="Text" textEncoding="utf-8" useDefaultWebProxy="true"
         allowCookies="false">
      <readerQuotas maxDepth="32" maxStringContentLength="2147483647" maxArrayLength="2147483647"
          maxBytesPerRead="409600" maxNameTableCharCount="16384" />
      <reliableSession ordered="true" inactivityTimeout="00:10:00"
          enabled="false" />
      <security mode="None">
        <transport clientCredentialType="Windows" proxyCredentialType="None"
            realm="" />
        <message clientCredentialType="Windows" negotiateServiceCredential="true"
            establishSecurityContext="true" />
      </security>
    </binding>
  </wsHttpBinding>
  </bindings>
  <client>
    <endpoint address="http://172.22.51.172:8080/Services.svc"
            binding="wsHttpBinding" bindingConfiguration="WSHttpBinding_WcfService"
            contract="ICT.RCS.Modules.Interface.IServices" name="Service" />
 </client>
</system.serviceModel>
  </configuration>
~~~
  
In the project "ICT.WPF.Client.MainFramework",we have a WPF window form named "MainWindow".When it loaded, we will run this code:

~~~ c#
	new ConnectionStringService().Approve();

	var subscriptionManager = SubscriptionManager.Default();

	subscriptionManager.Subscribe(new[] { typeof(OrderCompletedWindEvent).FullName });
	var bus = ServiceBus
		.Create(c => c.SubscriptionManager(subscriptionManager))
		.Start();

	Console.WriteLine("Sub A started.  Press CTRL+C to stop.");
	Console.WriteLine();
~~~
	
When the Service Bus instance is created, just like the `var bus = ServiceBus.Create(c => c.SubscriptionManager(subscriptionManager)).Start();`
It will send me this wrong message.
Do you know why?
	
### jabberwocky (Aug 26 at 7:52 PM)
	
Hello,

I have a sneaky suspicion it may have something to do with WPF and not Shuttle. It is not impossible that Shuttle has a hand in it I just cannot seem to replicate it. I'll e-mail you the sample that does exactly what you are describing and then we can tackle it through e-mail.

Regards,
Eben
	
### MoonSpring (Aug 27 at 8:28 AM)
	
Hi,

I've already done a demo: in a WPF project using the Shuttle.Esb,it can run normally.

Now when I add the WCF and Arcgis' configration into "App.config",it will send me some wrong messages,just like what I said above.

I've already debug it.When I create the Service Bus, just like "var bus = ServiceBus.Create(c => c.SubscriptionManager(subscriptionManager)).Start()" ,it will send me the wrong messages.
Maybe it is the problem about WCF or Arcgis.

Here is my e-mail adress : liu614005125@gmail.com.

Thank you very much for all your Support and help.

Regards,

Spring
	
### MoonSpring (Mon at 4:27 AM)
	
Hi,

sorry,man.I have to bother you again.I have tryed all the ways to find out the reason about the problem,but finally I failed.

I got to say,my dear god,I can't even find the reason why it will send me this wrong messages.

So if it is possible,we can talk with the remote connection.How about the QQ?

here is my QQ number:614005125.

Or you can tell me other chat tools.I need your help.

Thank you very much.
	
### jabberwocky (Mon at 6:04 AM)
	
Hi,

We can use Skype. My name is j.wocky

I will, unfortunately, only be able to assist you after 19h00 South African time. I do not know how that will impact you. As an alternative I may be able to do between 6h00 and 7h00 SA time one morning.

However, I do not know WPF at all but we can have a look.

Regards,
Eben

# About Starting a Service Bus 

### MoonSpring (Mon at 5:12 AM)
	
Hi,

How are you ?

I want to ask a quesion about starting a Service Bus.

In my project,I need the MainClient receives the message.
So I let the MaincClient part subcribe to event StartedEvent so it can handle the message. But at the same time,I have to start the Service Bus Instance for MainClient.

As far as I know,starting a Service Bus will take about 2 seconds.Can I change it faster?
	
### jabberwocky (Mon at 6:07 AM)
	
Hello,

The service bus start-up time will vary depending on a number of factors. There isn't any one thing that will make it go faster.

What is the actual problem, though. The start-up time should be impacting your processing.

Regards,
Eben

p.s. Is there no way that you can perhaps use the Shuttle.Esb issues page on GitHub for your questions as other may find it useful. I do not maintain the CodePlex side anymore and it will be deprecated (deleted) one day. The issue page is here: https://github.com/Shuttle/Shuttle.Esb/issues
	
### MoonSpring (Mon at 8:43 AM)
	
Hi,

OK,I will talk to you in the issue page next time,if I have problem about Shuttle.

At the same time ,this is so sad to hear that the records will be deleted.

I hope it will be saved forever.

