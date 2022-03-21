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
                text: 'Queue Manager',
                link: '/components/queue-manager'
            },
            {
                text: 'Policy',
                link: '/components/service-bus-policy'
            },
            {
                text: 'Subscription Manager',
                link: '/components/subscription-manager'
            },
            {
                text: 'Transport Message',
                link: '/components/transport-message'
            },
            {
                text: 'Transport Message Configurator',
                link: '/components/transport-message-configurator'
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
        text: 'Queue',
        items: [
            {
                text: 'Amazon SQS',
                link: '/implementations/queue/amazonsqs'
            },
            {
                text: 'Azure Storage Queues',
                link: '/implementations/queue/azuremq'
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
                text: 'Throttle (Windows)',
                link: '/modules/throttle'
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
            // algolia: {
            //     indexName: 'Shuttle.Core Documentation',
            //     appId: '42GX712C20',
            //     apiKey: 'cc059819a5d38ee7a55298f7cf9ed70a'
            // },

            // carbonAds: {
            //     code: '',
            //     placement: ''
            // },

            socialLinks: [
                { icon: 'github', link: 'https://github.com/Shuttle/shuttle-esb' },
                // { icon: 'twitter', link: '' },
                // { icon: 'discord', link: '' }
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
                    text: 'Configuration',
                    activeMatch: `^/configuration/`,
                    link: '/configuration/full'
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
            ],

            sidebar: {
                '/concepts/': concepts,
                '/guide/': guide,
                '/components/': components,
                '/implementations/': implementations,
                '/modules/': modules
            }
        },
    };
})()
