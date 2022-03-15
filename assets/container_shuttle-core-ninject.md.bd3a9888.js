import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.bd65a23a.js";
const __pageData = '{"title":"Ninject","description":"","frontmatter":{"title":"Ninject","layout":"api"},"headers":[],"relativePath":"container/shuttle-core-ninject.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="shuttle-core-ninject" tabindex="-1">Shuttle.Core.Ninject <a class="header-anchor" href="#shuttle-core-ninject" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Core.Ninject\n</code></pre></div><p>The <code>NinjectComponentContainer</code> implements both the <code>IComponentRegistry</code> and <code>IComponentResolver</code> interfaces.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#F78C6C;">var</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">container</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">new</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">NinjectComponentContainer</span><span style="color:#89DDFF;">(</span><span style="color:#F78C6C;">new</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">StandardKernel</span><span style="color:#89DDFF;">());</span></span>\n<span class="line"></span></code></pre></div>', 4);
const _hoisted_5 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_5);
}
var shuttleCoreNinject = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, shuttleCoreNinject as default };
