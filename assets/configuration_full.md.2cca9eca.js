import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.ed48be56.js";
const __pageData = '{"title":"Full Application Configuration File","description":"","frontmatter":{"sidebar":false,"aside":false},"headers":[],"relativePath":"configuration/full.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="full-application-configuration-file" tabindex="-1">Full Application Configuration File <a class="header-anchor" href="#full-application-configuration-file" aria-hidden="true">#</a></h1><p>This configuration is the default application configuration file based version. Some components that you may replace could very well have their own configuration stores.</p><p>To start off add the <code>Shuttle.Esb.ServiceBusSection</code> configuration class from the <code>Shuttle.Esb</code> assembly.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">section</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&#39;</span><span style="color:#C3E88D;">serviceBus</span><span style="color:#89DDFF;">&#39;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.ServiceBusSection, Shuttle.Esb</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">/&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>It is also possible to group the Shuttle configuration in a <code>shuttle</code> group:</p><div class="language-xml"><pre><code><span class="line"><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">sectionGroup</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">shuttle</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">            </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">section</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&#39;</span><span style="color:#C3E88D;">serviceBus</span><span style="color:#89DDFF;">&#39;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.ServiceBusSection, Shuttle.Esb</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">/&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">sectionGroup</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>The most pertinent bit is the <code>serviceBus</code> tag.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">serviceBus</span></span>\n<span class="line"><span style="color:#89DDFF;">    </span><span style="color:#C792EA;">cacheIdentity</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">true</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">  </span></span>\n<span class="line"><span style="color:#89DDFF;">    </span><span style="color:#C792EA;">createQueues</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">true</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">  </span></span>\n<span class="line"><span style="color:#89DDFF;">    </span><span style="color:#C792EA;">removeMessagesNotHandled</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">false</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">    </span><span style="color:#C792EA;">removeCorruptMessages</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">false</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">    </span><span style="color:#C792EA;">compressionAlgorithm</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">    </span><span style="color:#C792EA;">encryptionAlgorithm</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><table><thead><tr><th>Attribute</th><th>Default</th><th>Description</th></tr></thead><tbody><tr><td>registerHandlers</td><td>true</td><td>Will call the <code>RegisterHandlers</code> method on the <code>IMessageHandlerFactory</code> implementation if set to <code>true</code>.</td></tr><tr><td>cacheIdentity</td><td>true</td><td>Determines whether or not to re-use the identity returned by the <code>IIdentityProvider</code>.</td></tr><tr><td>createQueues</td><td>true</td><td>The endpoint will attempt to create all local queues (inbox, outbox, control inbox)</td></tr><tr><td>removeMessagesNotHandled</td><td>false</td><td>Indicates whether messages received on the endpoint that have no message handler should simply be removed (ignored). If this attribute is <code>true</code> the message will simply be acknowledged; else the message will immmediately be placed in the error queue. <em>The default changed from <strong>true</strong> to <strong>false</strong> in v7.0.1</em>.</td></tr><tr><td>removeCorruptMessages</td><td>false</td><td>A message is corrupt when the <code>TransportMessage</code> retrieved from the queue cannot be deserialized. If <code>false</code> (default) the service bus processed will be killed. If <code>true</code> the messae will be <code>Acknowledged</code> with no processing.</td></tr><tr><td>encryptionAlgorithm</td><td>empty (no encryption)</td><td>The name of the encryption algorithm to use when sending messages. Out-of-the-box there is a Triple DES implementation (class TripleDesEncryptionAlgorithm and name &#39;3DES&#39;).</td></tr></tbody></table><p>The <code>IIdentityProvider</code> implementation is responsible for honouring the <code>cacheIdentity</code> attribute.</p><p>Use the <code>queueFactories</code> tag to configure how you would like to locate queue factories. By default the current <code>AppDomain</code> is scanned for implementations of <code>IQueueFactory</code> along with all assemblies in the base directory (recursively). These queue factories have to have a parameterless constructor in order to be instantiated.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">queueFactories</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">scan</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">true|false</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.Msmq.MsmqQueueFactory, Shuttle.Esb.Msmq</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.RabbitMQ.RabbitMQQueueFactory, Shuttle.Esb.RabbitMQ</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.SqlServer.SqlQueueFactory, Shuttle.Esb.SqlServer</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">queueFactories</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>The <code>messageRoutes</code> tag defines the routing for message that are sent using the <code>IServiceBus.Send</code> method. You will notice that the structure is the same as the <code>forwardingRoutes</code> tag.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">messageRoutes</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">uri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./inbox</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">specification</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">StartsWith</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">value</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Messages1</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">specification</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">StartsWith</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">value</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Messages2</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">uri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">sql://./inbox</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">specification</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">TypeList</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">value</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">DoSomethingCommand</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">messageRoutes</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>The <code>inbox</code> should be specified if the endpoint has message handlers that need to process incoming messages.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">inbox</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">workQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./inbox-work</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">deferredQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./inbox-work-deferred</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">errorQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./shuttle-error</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">threadCount</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">25</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">durationToSleepWhenIdle</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">250ms,500ms,1s,5s</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">durationToIgnoreOnFailure</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">30m,1h</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">maximumFailureCount</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">25</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">distribute</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">true|false</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">distributeSendCount</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">5</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"></span></code></pre></div><table><thead><tr><th>Attribute</th><th>Default</th><th>Description</th></tr></thead><tbody><tr><td>threadCount</td><td>5</td><td>The number of worker threads that will service the inbox work queue. The deferred queue will always be serviced by only 1 thread.</td></tr><tr><td>durationToSleepWhenIdle</td><td>250ms,500ms,1s,5s</td><td>Uses the <a href="https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-timespantypeconverters.html" target="_blank" rel="noopener noreferrer">StringDurationArrayConverter</a> to convert to an array of <code>TimeSpan</code> instances. Specify <code>ms</code> (milliseconds), <code>s</code> (seconds), <code>m</code> (minutes), <code>h</code> (hours), <code>d</code> (days) * (times) <code>count</code>.</td></tr><tr><td>durationToIgnoreOnFailure</td><td>5m,30m,60m</td><td>Uses the <a href="https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-timespantypeconverters.html" target="_blank" rel="noopener noreferrer">StringDurationArrayConverter</a> to convert to an array of <code>TimeSpan</code> instances. Specify <code>ms</code> (milliseconds), <code>s</code> (seconds), <code>m</code> (minutes), <code>h</code> (hours), <code>d</code> (days) * (times) <code>count</code>.</td></tr><tr><td>maximumFailureCount</td><td>5</td><td>The maximum number of failures that are retried before the message is moved to the error queue.</td></tr><tr><td>distribute</td><td>false</td><td>If <code>true</code> the endpoint will act as only a distributor. If <code>false</code> the endpoint will distribute messages if a worker is available; else process the message itself.</td></tr><tr><td>distributeSendCount</td><td>5</td><td>The number of messages to send to the worker per available thread message received. If less than 1 the default will be used.</td></tr></tbody></table><p>For some queueing technologies the <code>outbox</code> may not be required. Msmq, for instance, create its own outgoing queues. However, it should be used in scenarios where you need a store-and-forward mechanism for sending messages when the underlying infrastructure does not provide this such as with a SqlServer table-based queue or maybe even the file system. RabbitMQ will also need an outbox since the destination broker may not be available and it does not have the concept of outgoing queues.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">outbox</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">workQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./outbox-work</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">errorQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./shuttle-error</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">durationToSleepWhenIdle</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">250ms,10s,30s</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">durationToIgnoreOnFailure</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">30m,1h</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">maximumFailureCount</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">25</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">threadCount</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">5</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"></span></code></pre></div><table><thead><tr><th>Attribute</th><th>Default</th><th>Description</th></tr></thead><tbody><tr><td>threadCount</td><td>1</td><td>The number of worker threads that will service the outbox work queue.</td></tr><tr><td>durationToSleepWhenIdle</td><td>250ms,500ms,1s,5s</td><td>Uses the <a href="https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-timespantypeconverters.html" target="_blank" rel="noopener noreferrer">StringDurationArrayConverter</a> to convert to an array of <code>TimeSpan</code> instances. Specify <code>ms</code> (milliseconds), <code>s</code> (seconds), <code>m</code> (minutes), <code>h</code> (hours), <code>d</code> (days) * (times) <code>count</code>.</td></tr><tr><td>durationToIgnoreOnFailure</td><td>5m,30m,60m</td><td>Uses the <a href="https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-timespantypeconverters.html" target="_blank" rel="noopener noreferrer">StringDurationArrayConverter</a> to convert to an array of <code>TimeSpan</code> instances. Specify <code>ms</code> (milliseconds), <code>s</code> (seconds), <code>m</code> (minutes), <code>h</code> (hours), <code>d</code> (days) * (times) <code>count</code>.</td></tr><tr><td>maximumFailureCount</td><td>5</td><td>The maximum number of failures that are retried before the message is moved to the error queue.</td></tr></tbody></table><p>When the endpoint is not a physical endpoint but rather a worker use the <code>worker</code> tag to specify the relevant configuration.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">worker</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">distributorControlWorkQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./control-inbox-work</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">threadAvailableNotificationIntervalSeconds</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">5</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"></span></code></pre></div><table><thead><tr><th>Attribute</th><th>Default</th><th>Description</th></tr></thead><tbody><tr><td>distributorControlWorkQueueUri</td><td>n/a</td><td>The control work queue uri of the distributor endpoint that this endpoint can handle messages for.</td></tr><tr><td>threadAvailableNotificationIntervalSeconds</td><td>15</td><td>The number of seconds to wait on an idle thread before notifying the distributor of availability <em>again</em></td></tr></tbody></table><p>Since a worker sends thread availability to the physical distribution master the distributor needs to have a special inbox called the control inbox that is used for these notifications.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">control</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">workQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">control-inbox-work</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">errorQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./shuttle-error</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">threadCount</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">25</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">durationToSleepWhenIdle</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">250ms,10s,30s</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">durationToIgnoreOnFailure</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">30m,1h</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">maximumFailureCount</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">25</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"></span></code></pre></div><table><thead><tr><th>Attribute</th><th>Default</th><th>Description</th></tr></thead><tbody><tr><td>threadCount</td><td>1</td><td>The number of worker thread that will service the control work queue.</td></tr><tr><td>durationToSleepWhenIdle</td><td>250ms,500ms,1s,5s</td><td>Uses the <a href="https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-timespantypeconverters.html" target="_blank" rel="noopener noreferrer">StringDurationArrayConverter</a> to convert to an array of <code>TimeSpan</code> instances. Specify <code>ms</code> (milliseconds), <code>s</code> (seconds), <code>m</code> (minutes), <code>h</code> (hours), <code>d</code> (days) * (times) <code>count</code>.</td></tr><tr><td>durationToIgnoreOnFailure</td><td>5m,30m,60m</td><td>Uses the <a href="https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-timespantypeconverters.html" target="_blank" rel="noopener noreferrer">StringDurationArrayConverter</a> to convert to an array of <code>TimeSpan</code> instances. Specify <code>ms</code> (milliseconds), <code>s</code> (seconds), <code>m</code> (minutes), <code>h</code> (hours), <code>d</code> (days) * (times) <code>count</code>.</td></tr><tr><td>maximumFailureCount</td><td>5</td><td>The maximum number of failures that are retried before the message is moved to the error queue.</td></tr></tbody></table><p>Use the <code>modules</code> tag to configure modules that can be loaded at runtime. These modules have to have a parameterless constructor in order to be instantiated; else add them programmatically if you need to specify parameters.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">modules</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.Modules.ActiveTimeRangeModule, Shuttle.Esb.Modules</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">modules</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>If you need to make use of the <code>DefaultUriResolver</code> you can specify the mappings as follows:</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">uriResolver</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">resolver://host/queue-1</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">uri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./inbox-work-queue</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">resolver://host/queue-2</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">uri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">rabbitmq://user:password@the-server/inbox-work-queue</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">uriResolver</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>Finally just close the relevant tags.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">serviceBus</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>You may wish to consider using the <a href="https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-transactions.html" target="_blank" rel="noopener noreferrer">TransactionScope</a> section to configure transactional behaviour for your endpoint.</p>', 33);
const _hoisted_34 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_34);
}
var full = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, full as default };
