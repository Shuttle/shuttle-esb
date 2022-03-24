import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.2d1f51b6.js";
const __pageData = '{"title":"IIdentityProvider","description":"","frontmatter":{},"headers":[{"level":2,"title":"Methods","slug":"methods"},{"level":3,"title":"Get","slug":"get"}],"relativePath":"components/identity-provider.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="iidentityprovider" tabindex="-1">IIdentityProvider <a class="header-anchor" href="#iidentityprovider" aria-hidden="true">#</a></h1><p>An implementation of the <code>IIdentityProvider</code> interface is used to obtain the <code>IIdentity</code> instance to use to return the <code>PrincipalIdentityName</code> of the [TransportMessage].</p><p>There is a <code>DefaultIdentityProvider</code> that is used if no other instance is provided.</p><h2 id="methods" tabindex="-1">Methods <a class="header-anchor" href="#methods" aria-hidden="true">#</a></h2><h3 id="get" tabindex="-1">Get <a class="header-anchor" href="#get" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IIdentity</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Get</span><span style="color:#89DDFF;">();</span></span>\n<span class="line"></span></code></pre></div><p>The method will return the <code>IIdentity</code> instance to use.</p><p><em>Note</em>: the <code>IIdentityProvider</code> implementation is responsible for honouring the <code>IServiceBusConfiguration.CacheIdentity</code> property.</p>', 8);
const _hoisted_9 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_9);
}
var identityProvider = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, identityProvider as default };
