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
<div class='row'>
    <div class='col-sm-6 col-md-4'>
        <div class='thumbnail'>
            <div class='caption'>
            <h3>Shuttle.Esb.Sql.Queue</h3>
            <p>Contains a Sql table-based <strong>queue</strong> implementation.</p>
            <p>
                <a href='http://www.nuget.org/packages/Shuttle.Esb.Sql.Queue' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
                <a href='https://github.com/Shuttle/Shuttle.Esb.Sql.Queue' target='_blank' class='btn btn-default' role='button'>GitHub</a>
                <a href='https://github.com/Shuttle/Shuttle.Esb.Sql.Queue/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
            </p>
            </div>
        </div>
    </div>
    <div class='col-sm-6 col-md-4'>
        <div class='thumbnail'>
            <div class='caption'>
            <h3>Shuttle.Esb.Sql.Subscription</h3>
            <p>Contains a Sql <strong>subscription manager</strong> implementation.</p>
            <p>
                <a href='http://www.nuget.org/packages/Shuttle.Esb.Sql.Subscription' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
                <a href='https://github.com/Shuttle/Shuttle.Esb.Sql.Subscription' target='_blank' class='btn btn-default' role='button'>GitHub</a>
                <a href='https://github.com/Shuttle/Shuttle.Esb.Sql.Subscription/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
            </p>
            </div>
        </div>
    </div>
    <div class='col-sm-6 col-md-4'>
        <div class='thumbnail'>
            <div class='caption'>
            <h3>Shuttle.Esb.Sql.Idempotence</h3>
            <p>Contains a Sql <strong>idempotence service</strong> implementation.</p>
            <p>
                <a href='http://www.nuget.org/packages/Shuttle.Esb.Sql.Idempotence' target='_blank' class='btn btn-primary' role='button'>NuGet</a>
                <a href='https://github.com/Shuttle/Shuttle.Esb.Sql.Idempotence' target='_blank' class='btn btn-default' role='button'>GitHub</a>
                <a href='https://github.com/Shuttle/Shuttle.Esb.Sql.Idempotence/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
            </p>
            </div>
        </div>
    </div>
</div>


<h2>Modules / Related</h2>

<div class='row'>
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
</div>

<div class='row'>
    <div class='col-sm-6 col-md-4'>
        <div class='thumbnail'>
            <div class='caption'>
            <h3>Shuttle.Esb.Process</h3>
            <p>Process management using Shuttle.Recall event sourcing to store process manager state.</p>
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
            <h3>Shuttle.Sentinel</h3>
            <p>Contains a management shell to manage various shuttle-specific bits such as moving messages and configuring subscriptions.</p>
            <p>
                <a href='https://github.com/Shuttle/Shuttle.Sentinel' target='_blank' class='btn btn-default' role='button'>GitHub</a>
                <a href='https://github.com/Shuttle/Shuttle.Sentinel/issues' target='_blank' class='btn btn-default' role='button'>Issues</a>
                <a href='http://shuttle.github.io/shuttle-recall/' target='_blank' class='btn btn-default' role='button'>Sentinel</a>
            </p>
            </div>
        </div>
    </div>
</div>
