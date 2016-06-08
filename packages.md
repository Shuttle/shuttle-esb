---
layout: default
title: Packages
---

<h2>Core Components</h2>
<div class='row'>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb</h3>
			<p>This is the core assembly and is always referenced when requiring a service bus instance.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
			</p>
			</div>
		</div>
	</div>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Core.Host</h3>
			<p>A generic host that can run as a console application or be installed as a Windows service.  Used to host Shuttle.Esb endpoints but can be used to host any Windows service.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Core.Host' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Core.Host' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Core.Host/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
			</p>
			</div>
		</div>
	</div>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.Samples</h3>
			<p>Although not really a core component you may want to take a look at these samples to get going since they illustrate some basic usage scenarios.</p>
			<p>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Samples' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Samples/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
			</p>
			</div>
		</div>
	</div>
</div>

<h2>Queues / Subscription / Idempotence</h2>
<div class='row'>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.RabbitMQ</h3>
			<p>Contains a RabbitMQ <strong>queue</strong> implementation.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb.RabbitMQ' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.RabbitMQ' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.RabbitMQ/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
				<a href='http://www.rabbitmq.com/' target='_blank' class='btn btn-default' role='button'>RabbitMQ</a>
			</p>
			</div>
		</div>
	</div>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.Msmq</h3>
			<p>Contains an MSMQ <strong>queue</strong> implementation.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb.Msmq' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Msmq' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Msmq/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
			</p>
			</div>
		</div>
	</div>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.SqlServer</h3>
			<p>Contains a Microsoft SqlServer table-based <strong>queue</strong>, <strong>subscription manager</strong>, and <strong>idempotence service</strong> implementation.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb.SqlServer' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.SqlServer' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.SqlServer/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
			</p>
			</div>
		</div>
	</div>
</div>
<div class='row'>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.FileMQ</h3>
			<p>Contains a file-based <strong>queue</strong> implementation.  <em>This is not intended for production use but rather to copy/back-up messages to the file-system</em>.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb.FileMQ' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.FileMQ' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.FileMQ/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
			</p>
			</div>
		</div>
	</div>
</div>

<h2>Dependency Injection Containers</h2>
<div class='row'>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.Castle</h3>
			<p>Contains a <em>WindsorContainer</em> implementation of the <strong>IMessageHandlerFactory</strong> interface.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb.Castle' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Castle' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Castle/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
				<a href='http://www.castleproject.org/projects/windsor/' target='_blank' class='btn btn-default' role='button'>Castle</a>
			</p>
			</div>
		</div>
	</div>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.Unity</h3>
			<p>Contains a <em>UnityContainer</em> implementation of the <strong>IMessageHandlerFactory</strong> interface.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb.Unity' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Unity' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Unity/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
				<a href='https://github.com/unitycontainer/unity' target='_blank' class='btn btn-default' role='button'>Unity</a>
			</p>
			</div>
		</div>
	</div>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.Ninject</h3>
			<p>Contains a <em>Ninject</em> implementation of the <strong>IMessageHandlerFactory</strong> interface.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb.Ninject' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Ninject' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Ninject/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
				<a href='http://www.ninject.org/' target='_blank' class='btn btn-default' role='button'>Ninject</a>
			</p>
			</div>
		</div>
	</div>
</div>

<h2>Extensions</h2>
<div class='row'>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.Process</h3>
			<p>Process management using shuttle-recall event sourcing.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb.Process' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Process' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Process/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
			</p>
			</div>
		</div>
	</div>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.Modules</h3>
			<p>Contains a number of Shuttle.Esb modules that extend Shuttle.Esb behaviour.</p>
			<p>
				<a href='http://www.nuget.org/packages/Shuttle.Esb.Modules' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Modules' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Modules/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
			</p>
			</div>
		</div>
	</div>
	<div class='col-sm-6 col-md-4'>
		<div class='thumbnail'>
			<div class='caption'>
			<h3>Shuttle.Esb.Management</h3>
			<p>Contains a management shell to manage various shuttle-specific bits such as moving messages and configuring subscriptions.</p>
			<p>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Management' target='_blank' class='btn btn-default' role='button'>GitHub</a>
				<a href='https://github.com/Shuttle/Shuttle.Esb.Management/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
			</p>
			</div>
		</div>
	</div>
</div>
