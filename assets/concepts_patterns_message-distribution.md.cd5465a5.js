import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.2d1f51b6.js";
var _imports_0 = "/shuttle-esb/images/message-distribution.png";
const __pageData = '{"title":"Message Distribution","description":"","frontmatter":{},"headers":[{"level":2,"title":"Message Distribution Exceptions","slug":"message-distribution-exceptions"}],"relativePath":"concepts/patterns/message-distribution.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="message-distribution" tabindex="-1">Message Distribution <a class="header-anchor" href="#message-distribution" aria-hidden="true">#</a></h1><p>It is conceivable that an endpoint can start falling behind with its processing if it receives too much work. In such cases it may be changed to distribute messages to worker nodes.</p><p><img src="' + _imports_0 + '" alt=""></p><p>An endpoint will automatically distribute messages to workers if it receives a worker availability message. An endpoint can be configured to only distribute messages, and therefore not process any messages itself, by setting the <code>distribute</code> attribute of the <code>inbox</code> configuration tag to <code>true</code>.</p><p>Since message distribution is integrated into the inbox processing the same endpoint simply needs to be installed aa many times as required on different machines as workers. The endpoint that you would like to have messages distributed on would require a control inbox configuration since all Shuttle messages should be processed without waiting in a queue like the inbox proper behind potentially thousands of messages. Each worker is identified as such in its configuration and the control inbox of the endpoint performing the distribution is required:</p><div class="language-xml"><pre><code><span class="line"><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">section</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">serviceBus</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.ServiceBusSection, Shuttle.Esb</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">/&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">serviceBus</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">control</span><span style="color:#89DDFF;"> </span></span>\n<span class="line"><span style="color:#89DDFF;">          </span><span style="color:#C792EA;">workQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./control-inbox-work</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span></span>\n<span class="line"><span style="color:#89DDFF;">          </span><span style="color:#C792EA;">errorQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./shuttle-error</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">/&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">inbox</span><span style="color:#89DDFF;"> </span></span>\n<span class="line"><span style="color:#89DDFF;">          </span><span style="color:#C792EA;">distribute</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">true</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">          </span><span style="color:#C792EA;">workQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./inbox-work</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span></span>\n<span class="line"><span style="color:#89DDFF;">          </span><span style="color:#C792EA;">errorQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./shuttle-error</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">/&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">serviceBus</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>Any endpoint that receives messages can be configured to include message distribution.</p><p>You then install as many workers as you require on as many machines as you want to and configure them to talk to a distributor. The physical distributor along with all the related workers form the logical endpoint for a message. The worker configuration is as follows:</p><div class="language-xml"><pre><code><span class="line"><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">section</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">serviceBus</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.ServiceBusSection, Shuttle.Esb</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">/&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">serviceBus</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">worker</span></span>\n<span class="line"><span style="color:#89DDFF;">         </span><span style="color:#C792EA;">distributorControlWorkQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq:///control-inbox=work</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">inbox</span></span>\n<span class="line"><span style="color:#89DDFF;">         </span><span style="color:#C792EA;">workQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./workerN-inbox-work</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">         </span><span style="color:#C792EA;">errorQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://./shuttle-error</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">         </span><span style="color:#C792EA;">threadCount</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">15</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">inbox</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">serviceBus</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>As soon as the application configuration file contains the <strong>worker</strong> tag each thread that goes idle will send a message to the distributor to indicate that a thread has become available to perform word. The distributor will then send one or more message for each available thread. The number of messages sent for each availability message is configured using the <code>distributeSendCount</code> attribute of the <code>inbox</code> tag.</p><h2 id="message-distribution-exceptions" tabindex="-1">Message Distribution Exceptions <a class="header-anchor" href="#message-distribution-exceptions" aria-hidden="true">#</a></h2><p>Some queueing technologies do not require message distribution. Instead of a worker another instance of the endpoint can consume the same input queue. This mechanism applies to brokers. Since brokers manage queues centrally the messages are consumed via consumers typically running per thread. Where the consumers originates does not matter so the queue can be consumed from various processes.</p><p>The broker style differes from something like Msmq or Sql-based queues where the message-handling is managed by the process hosting the thread-consumers. Here <code>process-A</code> would not be aware of which messages are being consumed by <code>process-B</code> leading to one <em>stealing</em> messages from the other.</p>', 13);
const _hoisted_14 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_14);
}
var messageDistribution = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, messageDistribution as default };