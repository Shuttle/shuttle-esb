import { _ as _export_sfc, o as openBlock, c as createElementBlock, a as createStaticVNode } from "./app.c73b04f3.js";
const __pageData = JSON.parse('{"title":"Message Sender","description":"","frontmatter":{},"headers":[{"level":2,"title":"Methods","slug":"methods","link":"#methods","children":[{"level":3,"title":"Dispatch","slug":"dispatch","link":"#dispatch","children":[]},{"level":3,"title":"Send","slug":"send","link":"#send","children":[]},{"level":3,"title":"Publish","slug":"publish","link":"#publish","children":[]}]}],"relativePath":"components/message-sender.md"}');
const _sfc_main = { name: "components/message-sender.md" };
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="message-sender" tabindex="-1">Message Sender <a class="header-anchor" href="#message-sender" aria-hidden="true">#</a></h1><p>The purpose of the <code>IMessageSender</code> is to abstract sending and publishing capabilities. The <code>MessageSender</code> class provides the actual implementation and both the <code>ServiceBus</code> and <code>HandlerContext</code> classes hold a reference to a <code>MessageSender</code>.</p><h2 id="methods" tabindex="-1">Methods <a class="header-anchor" href="#methods" aria-hidden="true">#</a></h2><h3 id="dispatch" tabindex="-1">Dispatch <a class="header-anchor" href="#dispatch" aria-hidden="true">#</a></h3><div class="language-c#"><button class="copy"></button><span class="lang">c#</span><pre><code><span class="line"><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Dispatch</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessage</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessageReceived</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>This method invokes the <code>DispatchTransportMessagePipeline</code> to have the given <code>TransportMessage</code> eventually enqueued on the target queue as specified by the <code>RecipientInboxWorkQueueUri</code> of the <code>TransportMessage</code>. If this <code>Dispatch</code> takes place in response to the processing of received <code>TransportMessage</code> then the <code>transportMessageReceived</code> should be the received message; else it is <code>null</code>.</p><h3 id="send" tabindex="-1">Send <a class="header-anchor" href="#send" aria-hidden="true">#</a></h3><div class="language-c#"><button class="copy"></button><span class="lang">c#</span><pre><code><span class="line"><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Send</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessageReceived</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Action</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TransportMessageBuilder</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">builder</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Creates and then dispatches a <code>TransportMessage</code> using the message routing as configured. The newly instantiated <code>TransportMessage</code> is returned. The same <code>transportMessageReceived</code> processing applies as to the <code>Dispatch</code>. The <code>builder</code> allows you to customize the newly created <code>TransportMessage</code>.</p><h3 id="publish" tabindex="-1">Publish <a class="header-anchor" href="#publish" aria-hidden="true">#</a></h3><div class="language-c#"><button class="copy"></button><span class="lang">c#</span><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Publish</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessageReceived</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Action</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TransportMessageBuilder</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">builder</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Creates and then dispatches a <code>TransportMessage</code> to all URIs returned from the registered <code>ISubscriptionService</code>. The same <code>transportMessageReceived</code> processing applies as to the <code>Dispatch</code>. The <code>builder</code> allows you to customize the newly created <code>TransportMessage</code>.</p><p>All the instantiated <code>TransportMessage</code> instances are returned, with one message for each of relevant <code>RecipientInboxWorkQueueUri</code> that was subscribed to.</p>', 13);
const _hoisted_14 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_14);
}
const messageSender = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export {
  __pageData,
  messageSender as default
};
