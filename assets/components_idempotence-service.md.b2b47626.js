import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.ed48be56.js";
const __pageData = '{"title":"Idempotence Service","description":"","frontmatter":{},"headers":[{"level":2,"title":"Methods","slug":"methods"},{"level":3,"title":"ProcessingStatus","slug":"processingstatus"},{"level":3,"title":"ProcessingCompleted","slug":"processingcompleted"},{"level":3,"title":"AddDeferredMessage","slug":"adddeferredmessage"},{"level":3,"title":"GetDeferredMessages","slug":"getdeferredmessages"},{"level":3,"title":"DeferredMessageSent","slug":"deferredmessagesent"},{"level":3,"title":"MessageHandled","slug":"messagehandled"}],"relativePath":"components/idempotence-service.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="idempotence-service" tabindex="-1">Idempotence Service <a class="header-anchor" href="#idempotence-service" aria-hidden="true">#</a></h1><p>An implementation of the <code>IIdempotenceService</code> interface is responsible for ensuring that message remain idempotent on a technical level. This means that if, by some edge case, a message happens to be duplicated then only one instance of the message will be processed. This is done by keeping track of which message ids have been processed.</p><p>In addition to this the idempotence service also defers message sending when message are sent (or published) within a transaction.</p><h2 id="methods" tabindex="-1">Methods <a class="header-anchor" href="#methods" aria-hidden="true">#</a></h2><h3 id="processingstatus" tabindex="-1">ProcessingStatus <a class="header-anchor" href="#processingstatus" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">ProcessingStatus</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">ProcessingStatus</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessage</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>This method must return the <code>ProcessingStatus</code> of the given <code>TransportMessage</code>:</p><ul><li>Returns <code>ProcessingStatus.Ignore</code> if the message has been <strong>processed</strong> completely and also if it currently being processed by another consumer.</li><li>Returns <code>ProcessingStatus.MessageHandled</code> if the message has already been handled. There may be deferred messages that need to be sent.</li><li>Returns <code>ProcessingStatus.Assigned</code> if this message is assigned for initial processing.</li></ul><h3 id="processingcompleted" tabindex="-1">ProcessingCompleted <a class="header-anchor" href="#processingcompleted" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">ProcessingCompleted</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessage</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Marks the message as having being processed successfully.</p><h3 id="adddeferredmessage" tabindex="-1">AddDeferredMessage <a class="header-anchor" href="#adddeferredmessage" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">AddDeferredMessage</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">processingTransportMessage</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Stream</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">deferredTransportMessageStream</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Saves the <code>deferredTransportMessageStream</code> against the given <code>processingTransportMessage</code> in order for the service bus to perform the actual dispatching of the deferred message after the messae ahs been handled.</p><h3 id="getdeferredmessages" tabindex="-1">GetDeferredMessages <a class="header-anchor" href="#getdeferredmessages" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">Stream</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetDeferredMessages</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessage</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Returns all the streams that were sent during the handling of the given <code>transportMesage</code>.</p><h3 id="deferredmessagesent" tabindex="-1">DeferredMessageSent <a class="header-anchor" href="#deferredmessagesent" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">DeferredMessageSent</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">processingTransportMessage</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">deferredTransportMessage</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>This method should remove the entry associated with the <code>deferredTransportMessage</code> as it has been dispatched.</p><h3 id="messagehandled" tabindex="-1">MessageHandled <a class="header-anchor" href="#messagehandled" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">MessageHandled</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">TransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessage</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Once the message has been successfully handled this method is called to mark the message as handled in the store.</p>', 23);
const _hoisted_24 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_24);
}
var idempotenceService = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, idempotenceService as default };
