import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.ed48be56.js";
const __pageData = '{"title":"Message Routing","description":"","frontmatter":{},"headers":[{"level":2,"title":"Implementation","slug":"implementation"}],"relativePath":"concepts/patterns/message-routing.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="message-routing" tabindex="-1">Message Routing <a class="header-anchor" href="#message-routing" aria-hidden="true">#</a></h1><p>Once you have instantiated a message you need to get it to a specific endpoint. You can let Shuttle.Esb decide this for you implicitly by configuring a routing mechanism or you can even specify the endpoint explicitly.</p><p>Typically when sending a message that message is a command. It does not <em>have</em> to be a command and you <em>can</em> send an event message to a specific endpoint but more-often-than-not you will be sending a command. Messages are sent by calling one of the relevant overloaded methods on the service bus instance:</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Send</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Send</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Action</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TransportMessageConfigurator</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">configure</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Only messages that have no <code>RecipientInboxWorkQueueUri</code> set will be routed by the service bus.</p><p>The <code>TransportMessage</code> envelope will be returned should you need access to any of the metadata available for the message.</p><p>Shuttle.Esb uses an implementation of an <code>IMessageRouteProvider</code> to determine where messages are sent.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">interface</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">IMessageRouteProvider</span></span>\n<span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;string&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetRouteUris</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">);</span><span style="color:#A6ACCD;">    </span></span>\n<span class="line"><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span></code></pre></div><h2 id="implementation" tabindex="-1">Implementation <a class="header-anchor" href="#implementation" aria-hidden="true">#</a></h2><p>The <code>DefaultMessageRouteProvider</code> is registered if no <code>IMessageRouteProvider</code> has been registered and makes use of the application configuration file to determine where to send messages:</p><div class="language-xml"><pre><code><span class="line"><span style="color:#89DDFF;">&lt;?</span><span style="color:#F07178;">xml</span><span style="color:#C792EA;"> version</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">1.0</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C792EA;"> encoding</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">utf-8</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> ?&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">section</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">serviceBus</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.ServiceBusSection, Shuttle.Esb</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">/&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">serviceBus</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">messageRoutes</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">         </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">uri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://serverA/inbox</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">            </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">specification</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">StartsWith</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">value</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Messages1</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">            </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">specification</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">StartsWith</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">value</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Messages2</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">         </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">         </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">uri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">sql://serverB/inbox</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">            </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">specification</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">TypeList</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">value</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">DoSomethingCommand, Assembly</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">         </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">         </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">uri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">msmq://serverC/inbox</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">            </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">specification</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Regex</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">value</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">.+[Cc]ommand.+</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">         </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">         </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">uri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">sql://serverD/inbox</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">            </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">add</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">specification</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Assembly</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">value</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">TheAssemblyName</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">         </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">messageRoute</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">messageRoutes</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">   </span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">serviceBus</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>Each implementation of <code>IMessageRouteProvider</code> can determine the routes however it needs to from the given message. A typical scenario, and the way the <code>DefaultMessageRouteProvider</code> works, is to use the full type name to determine the destination.</p><p><strong>Please note</strong>: each message type may only be sent to <em>one</em> endpoint (using <code>Send</code>).</p>', 13);
const _hoisted_14 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_14);
}
var messageRouting = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, messageRouting as default };
