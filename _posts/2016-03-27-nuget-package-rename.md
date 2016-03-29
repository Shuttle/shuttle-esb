---
title: NuGet Package Rename
layout: post
---

All packages have been renamed to the .Net assembly named standard.  No more dashes such as `shuttle-esb-rabbitmq` and also no more than two uppercase characters for acronyms.

# shuttle-core-infrastructure-log4net

This assembly has been renamed to `Shuttle.Core.Log4Net`.  Note that the `Infrastructure` has been dropped.

# shuttle-esb-core

This assembly is now only `Shuttle.Esb`.  No more `.Core` and the `ESB` has become `Esb`.  You may need to search/replace in all files.

# ESB to Esb

Since the `ESB` part has been change to `Esb` you need to search/replace all files including configuration files.

The configuration section is top of this list:

~~~xml
   <configSections>
      <section name="serviceBus" type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
   </configSections>
~~~

Would become (note ESB and dropped Core):

~~~xml
   <configSections>
      <section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
   </configSections>
~~~

Any other configuration section affected would also need to change.  The msmq configuration section, for instance, would need to `ESB` changed to `Esb`:

~~~xml
	<modules>
		<add type="Shuttle.ESB.Modules.ActiveTimeRangeModule, Shuttle.ESB.Modules" />
	</modules>
~~~

Will need to become (else some configuration loading exceptions may occur):

~~~xml
	<modules>
		<add type="Shuttle.ESB.Modules.ActiveTimeRangeModule, Shuttle.ESB.Modules" />
	</modules>
~~~

The same applies to any other configuration sections.
