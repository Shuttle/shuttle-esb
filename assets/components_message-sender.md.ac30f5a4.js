import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.ed48be56.js";
const __pageData = '{"title":"Message Sender","description":"","frontmatter":{},"headers":[{"level":2,"title":"Methods","slug":"methods"},{"level":3,"title":"CreateTransportMessage","slug":"createtransportmessage"},{"level":3,"title":"Dispatch","slug":"dispatch"},{"level":3,"title":"Send","slug":"send"},{"level":3,"title":"Publish","slug":"publish"}],"relativePath":"components/message-sender.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="message-sender" tabindex="-1">Message Sender <a class="header-anchor" href="#message-sender" aria-hidden="true">#</a></h1><p>The purpose of the <code>IMessageSender</code> is to abstract sending and publishing capabilities. The <code>MessageSender</code> class provides the actual implementation and both the <code>ServiceBus</code> and <code>HandlerContext</code> classes hold a reference to a <code>MessageSender</code>.</p><h2 id="methods" tabindex="-1">Methods <a class="header-anchor" href="#methods" aria-hidden="true">#</a></h2><h3 id="createtransportmessage" tabindex="-1">CreateTransportMessage <a class="header-anchor" href="#createtransportmessage" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">CreateTransportMessage</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Action</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TransportMessageConfigurator</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">configure</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Returns a new instance of a <code>TransportMessage</code> using the <code>TransportMessageConfigurator</code> provided by the <code>configure</code> action by invoking the <code>TransportMessagePipeline</code>.</p><h3 id="dispatch" tabindex="-1">Dispatch <a class="header-anchor" href="#dispatch" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Dispatch</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessage</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>This method invokes the <code>DispatchTransportMessagePipeline</code> to have the given <code>TransportMessage</code> eventually enqueued on the target queue as specified by the <code>RecipientInboxWorkQueueUri</code> of the <code>TransportMessage</code>.</p><h3 id="send" tabindex="-1">Send <a class="header-anchor" href="#send" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Send</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Creates and then dispatches a <code>TransportMessage</code> using the message routing as configured. The newly instantiated <code>TransportMessage</code> is returned.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Send</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Action</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TransportMessageConfigurator</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">configure</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Creates and then dispatches a <code>TransportMessage</code> using the <code>TransportMessageConfigurator</code> returned by the <code>configure</code> action. The newly instantiated <code>TransportMessage</code> is returned.</p><h3 id="publish" tabindex="-1">Publish <a class="header-anchor" href="#publish" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Publish</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Creates and then dispatches a <code>TransportMessage</code> for each uri returned by the registered <code>ISubscriptionManager</code> instance. The newly instantiated <code>TransportMessage</code> collection returned with one message for each of relevant <code>RecipientInboxWorkQueueUri</code> subscription Uris.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Publish</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Action</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TransportMessageConfigurator</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">configure</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Creates and then dispatches a <code>TransportMessage</code> for each uri returned by the registered <code>ISubscriptionManager</code> instance. There should be very few instances where this method will be required. The newly instantiated <code>TransportMessage</code> collection returned with one message for each of relevant <code>RecipientInboxWorkQueueUri</code> subscription Uris.</p>', 19);
const _hoisted_20 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_20);
}
var messageSender = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, messageSender as default };
