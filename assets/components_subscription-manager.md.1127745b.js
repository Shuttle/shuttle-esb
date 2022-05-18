import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.c706ac22.js";
const __pageData = '{"title":"Subscription Manager","description":"","frontmatter":{},"headers":[{"level":2,"title":"Methods","slug":"methods"},{"level":3,"title":"GetSubscribedUris","slug":"getsubscribeduris"}],"relativePath":"components/subscription-manager.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="subscription-manager" tabindex="-1">Subscription Manager <a class="header-anchor" href="#subscription-manager" aria-hidden="true">#</a></h1><p>An implementation of the <code>ISubscriptionManager</code> interface is used by endpoints to subscribe to message types and to get the endpoint uris that have subscribed to a message type.</p><p>There is no <em>default</em> implementation of the <code>ISubscriptionManager</code> interface as the data has to be persisted in some central data store.</p><h2 id="methods" tabindex="-1">Methods <a class="header-anchor" href="#methods" aria-hidden="true">#</a></h2><h3 id="getsubscribeduris" tabindex="-1">GetSubscribedUris <a class="header-anchor" href="#getsubscribeduris" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;string&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetSubscribedUris</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns a list of endpoint uris that have subscribed to the type name of the given message.</p>', 7);
const _hoisted_8 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_8);
}
var subscriptionManager = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, subscriptionManager as default };
