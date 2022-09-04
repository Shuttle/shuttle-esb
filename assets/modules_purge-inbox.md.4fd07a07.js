import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.28dce079.js";
const __pageData = '{"title":"Purge Inbox","description":"","frontmatter":{},"headers":[{"level":2,"title":"Configuration","slug":"configuration"}],"relativePath":"modules/purge-inbox.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="purge-inbox" tabindex="-1">Purge Inbox <a class="header-anchor" href="#purge-inbox" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Esb.Module.PurgeInbox\n</code></pre></div><p>The PurgeInbox module for Shuttle.Esb clears the inbox work queue upon startup.</p><p>The module will attach the <code>PurgeInboxObserver</code> to the <code>OnAfterConfigure</code> event of the <code>StartupPipeline</code> and purges the inbox work queue if the relevant queue implementation has implemented the <code>IPurgeQueue</code> interface. If the inbox work queue implementation has <em>not</em> implemented the <code>IPurgeQueue</code> interface the purge is ignored.</p><h2 id="configuration" tabindex="-1">Configuration <a class="header-anchor" href="#configuration" aria-hidden="true">#</a></h2><div class="language-c#"><pre><code><span class="line"><span style="color:#A6ACCD;">services</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">AddPurgeInboxModule</span><span style="color:#89DDFF;">();</span></span>\n<span class="line"></span></code></pre></div>', 6);
const _hoisted_7 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_7);
}
var purgeInbox = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, purgeInbox as default };
