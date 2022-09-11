const getBase = require('shuttle-theme/config')
const path = require('path')

const concepts = [
    {
        text: 'Concepts',
        items: [
            {
                text: 'Why?',
                link: '/concepts/why'
            },
            {
                text: 'Autonomous Business Components',
                link: '/concepts/autonomous-business-components'
            },
        ]
    },
    {
        text: 'Patterns',
        items: [
            {
                text: 'Stream Processing',
                link: '/concepts/patterns/stream-processing'
            },
            {
                text: 'Request / Response',
                link: '/concepts/patterns/request-response'
            },
            {
                text: 'Publish / Subscribe',
                link: '/concepts/patterns/publish-subscribe'
            },
            {
                text: 'Message Distribution',
                link: '/concepts/patterns/message-distribution'
            },
            {
                text: 'Message Idempotence',
                link: '/concepts/patterns/message-idempotence'
            },
            {
                text: 'Deferred Messages',
                link: '/concepts/patterns/deferred-messages'
            },
            {
                text: 'Message Routing',
                link: '/concepts/patterns/message-routing'
            },
        ]
    },
    {
        text: 'Essentials',
        items: [
            {
                text: 'Bounded Contexts',
                link: '/concepts/essentials/bounded-contexts'
            },
        ]
    },
];

const guide = [
    {
        text: 'Overview',
        items: [
            {
                text: 'Introduction',
                link: '/guide/introduction'
            },
            {
                text: 'Getting Started',
                link: '/guide/getting-started'
            },
        ]
    },
    {
        text: 'Patterns',
        items: [
            {
                text: 'Stream Processing',
                link: '/guide/patterns/stream-processing'
            },
            {
                text: 'Request / Response',
                link: '/guide/patterns/request-response'
            },
            {
                text: 'Publish / Subscribe',
                link: '/guide/patterns/publish-subscribe'
            },
            {
                text: 'Deferred Messages',
                link: '/guide/patterns/deferred-messages'
            },
            {
                text: 'Dependency Injection',
                link: '/guide/patterns/dependency-injection'
            },
            {
                text: 'Message Distribution',
                link: '/guide/patterns/message-distribution'
            },
            {
                text: 'Message Idempotence',
                link: '/guide/patterns/message-idempotence'
            },
            {
                text: 'Process Management',
                link: '/guide/patterns/process-management'
            },
        ]
    },
    {
        text: 'Essentials',
        items: [
            {
                text: 'Exception Handling',
                link: '/guide/essentials/exception-handling'
            },
        ]
    },
];

const components = [
    {
        text: 'Implementations',
        items: [
            {
                text: 'Idempotence Service',
                link: '/components/idempotence-service'
            },
            {
                text: 'Identity Provider',
                link: '/components/identity-provider'
            },
            {
                text: 'Message Handler Invoker',
                link: '/components/message-handler-invoker'
            },
            {
                text: 'Message Handler',
                link: '/components/message-handler'
            },
            {
                text: 'Message Handling Assessor',
                link: '/components/message-handling-assessor'
            },
            {
                text: 'Message Route Provider',
                link: '/components/message-route-provider'
            },
            {
                text: 'Message Sender',
                link: '/components/message-sender'
            },
            {
                text: 'Queue Factory Service',
                link: '/components/queue-factory-service'
            },
            {
                text: 'Queue Service',
                link: '/components/queue-service'
            },
            {
                text: 'Policy',
                link: '/components/service-bus-policy'
            },
            {
                text: 'Subscription Service',
                link: '/components/subscription-service'
            },
            {
                text: 'Transport Message',
                link: '/components/transport-message'
            },
            {
                text: 'Transport Message Builder',
                link: '/components/transport-message-builder'
            },
            {
                text: 'Transport Header',
                link: '/components/transport-header'
            },
        ]
    },
];

const implementations = [
    {
        items: [
            {
                text: 'Overview',
                link: '/implementations/overview'
            },
        ]
    },
    {
        text: 'Streams',
        items: [
            {
                text: 'Kafka',
                link: '/implementations/stream/kafka'
            },
        ]
    },
    {
        text: 'Queue',
        items: [
            {
                text: 'Amazon SQS',
                link: '/implementations/queue/amazonsqs'
            },
            {
                text: 'Azure Storage Queues',
                link: '/implementations/queue/azuresq'
            },
            {
                text: 'File',
                link: '/implementations/queue/filemq'
            },
            {
                text: 'MSMQ',
                link: '/implementations/queue/msmq'
            },
            {
                text: 'RabbitMQ',
                link: '/implementations/queue/rabbitmq'
            },
            {
                text: 'SQL',
                link: '/implementations/queue/sql'
            },
        ]
    },
    {
        text: 'Subscription',
        items: [
            {
                text: 'SQL',
                link: '/implementations/subscription/sql'
            },
        ]
    },
    {
        text: 'Idempotence',
        items: [
            {
                text: 'SQL',
                link: '/implementations/idempotence/sql'
            },
        ]
    },
];

const modules = [
    {
        text: 'Implementations',
        items: [
            {
                text: 'Active Time Range',
                link: '/modules/active-time-range'
            },
            {
                text: 'Corrupt Transport Message',
                link: '/modules/corrupt-transport-message'
            },
            {
                text: 'Message Forwarding',
                link: '/modules/message-forwarding'
            },
            {
                text: 'Purge Inbox',
                link: '/modules/purge-inbox'
            },
            {
                text: 'Purge Queues',
                link: '/modules/purge-queues'
            },
            {
                text: 'Throttle',
                link: '/modules/throttle'
            },
        ]
    },
];

const options = [
    {
        text: 'Options',
        items: [
            {
                text: 'Service Bus',
                link: '/options/servicebus'
            },
            {
                text: 'Inbox',
                link: '/options/inbox'
            },
            {
                text: 'Message Routes',
                link: '/options/message-routes'
            },
            {
                text: 'Outbox',
                link: '/options/outbox'
            },
            {
                text: 'Control Inbox',
                link: '/options/control-inbox'
            },
            {
                text: 'Worker',
                link: '/options/worker'
            },
            {
                text: 'Subscription',
                link: '/options/subscription'
            },
            {
                text: 'Idempotence',
                link: '/options/idempotence'
            },
            {
                text: 'Processor Thread',
                link: '/options/processor-thread'
            },
        ]
    },
];


module.exports = (async () => {
    const base = await getBase();

    return {
        ...base,

        vite: {
            ...base.vite,
            build: {
                minify: false
            },
        },

        base: '/shuttle-esb/',
        lang: 'en-US',
        title: 'Shuttle.Esb',
        description: 'Shuttle.Esb Documentation',

        head: [
            ...base.head,
            ['link', { rel: "shortcut icon", href: "/shuttle-esb/favicon.ico" }]
        ],

        themeConfig: {
            algolia: {
                indexName: 'shuttle-esb',
                appId: 'VM33RJ87BH',
                apiKey: 'fc560606a3c14b173d0ddf57a3258c84'
            },

            // carbonAds: {
            //     code: '',
            //     placement: ''
            // },

            socialLinks: [
                { icon: 'github', link: 'https://github.com/Shuttle/shuttle-esb' },
                { icon: 'discord', link: 'https://discord.gg/Q2yEsfht6f' }
                // { icon: 'twitter', link: '' },
            ],

            footer: {
                copyright: `Copyright © 2013-${new Date().getFullYear()} Eben Roux`
            },
            
            nav: [
                {
                    text: 'Concepts',
                    activeMatch: `^/concepts/`,
                    link: '/concepts/why'
                },
                {
                    text: 'Guide',
                    activeMatch: `^/guide/`,
                    link: '/guide/introduction'
                },
                {
                    text: 'Options',
                    activeMatch: `^/options/`,
                    link: '/options/servicebus'
                },
                {
                    text: 'Components',
                    activeMatch: `^/components/`,
                    link: '/components/overview'
                },
                {
                    text: 'Implementations',
                    activeMatch: `^/implementations/`,
                    link: '/implementations/overview'
                },
                {
                    text: 'Modules',
                    activeMatch: `^/modules/`,
                    link: '/modules/overview'
                },
                {
                    text: 'v13.1.0',
                    items: [
                        {
                            text: 'changelog',
                            link: '/v13.1.0',
                        },
                        {
                            text: 'v-previous',
                            link: 'https://shuttle.github.io/shuttle-esb-v12/',
                        }
                    ]
                },
            ],

            sidebar: {
                '/concepts/': concepts,
                '/guide/': guide,
                '/components/': components,
                '/options/': options,
                '/implementations/': implementations,
                '/modules/': modules
            }
        },
    };
})()
