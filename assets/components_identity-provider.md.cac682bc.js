import { _ as _export_sfc, o as openBlock, c as createElementBlock, a as createStaticVNode } from "./app.35c0796e.js";
const __pageData = JSON.parse('{"title":"IIdentityProvider","description":"","frontmatter":{},"headers":[{"level":2,"title":"Methods","slug":"methods","link":"#methods","children":[{"level":3,"title":"Get","slug":"get","link":"#get","children":[]}]}],"relativePath":"components/identity-provider.md"}');
const _sfc_main = { name: "components/identity-provider.md" };
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="iidentityprovider" tabindex="-1">IIdentityProvider <a class="header-anchor" href="#iidentityprovider" aria-hidden="true">#</a></h1><p>An implementation of the <code>IIdentityProvider</code> interface is used to obtain the <code>IIdentity</code> instance to use to return the <code>PrincipalIdentityName</code> of the <code>TransportMessage</code>.</p><p>There is a <code>DefaultIdentityProvider</code> that is used if no other instance is provided.</p><h2 id="methods" tabindex="-1">Methods <a class="header-anchor" href="#methods" aria-hidden="true">#</a></h2><h3 id="get" tabindex="-1">Get <a class="header-anchor" href="#get" aria-hidden="true">#</a></h3><div class="language-c#"><button class="copy"></button><span class="lang">c#</span><pre><code><span class="line"><span style="color:#FFCB6B;">IIdentity</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Get</span><span style="color:#89DDFF;">();</span></span>\n<span class="line"></span></code></pre></div><p>The method will return the <code>IIdentity</code> instance to use.</p><p><em>Note</em>: the <code>IIdentityProvider</code> implementation is responsible for honouring the <code>ServiceBusOptions.CacheIdentity</code> property.</p>', 8);
const _hoisted_9 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_9);
}
const identityProvider = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export {
  __pageData,
  identityProvider as default
};
