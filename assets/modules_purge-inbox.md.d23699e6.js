import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.c706ac22.js";
const __pageData = '{"title":"Purge Inbox","description":"","frontmatter":{},"headers":[{"level":2,"title":"Registration / Activation","slug":"registration-activation"}],"relativePath":"modules/purge-inbox.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="purge-inbox" tabindex="-1">Purge Inbox <a class="header-anchor" href="#purge-inbox" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Esb.Module.PurgeInbox\n</code></pre></div><p>The PurgeInbox module for Shuttle.Esb clears the inbox work queue upon startup.</p><p>The module will attach the <code>PurgeInboxObserver</code> to the <code>OnAfterInitializeQueueFactories</code> event of the <code>StartupPipeline</code> and purges the inbox work queue if the relevant queue implementation has implemented the <code>IPurgeQueue</code> interface. If the inbox work queue implementation has <em>not</em> implemented the <code>IPurgeQueue</code> interface only a warning is logged.</p><h2 id="registration-activation" tabindex="-1">Registration / Activation <a class="header-anchor" href="#registration-activation" aria-hidden="true">#</a></h2><p>The required components may be registered by calling <code>ComponentRegistryExtensions.RegisterPurgeInbox(IComponentRegistry)</code>.</p><p>In order for the module to attach to the <code>IPipelineFactory</code> you would need to resolve it using <code>IComponentResolver.Resolve&lt;PurgeInboxModule&gt;()</code>.</p>', 7);
const _hoisted_8 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_8);
}
var purgeInbox = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, purgeInbox as default };
