import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.bd43fc14.js";
const __pageData = '{"title":"Autofac","description":"","frontmatter":{"title":"Autofac","layout":"api"},"headers":[],"relativePath":"container/shuttle-core-autofac.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="shuttle-core-autofac" tabindex="-1">Shuttle.Core.Autofac <a class="header-anchor" href="#shuttle-core-autofac" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Core.Autofac\n</code></pre></div><p>The implementation for Autofac makes use of both a <code>ComponentRegistry</code> that implements the <code>IComponentRegistry</code> interface as well as an <code>ComponentResolver</code> that implements the <code>IComponentResolver</code> interface.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#F78C6C;">var</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">builder</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">new</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">ContainerBuilder</span><span style="color:#89DDFF;">();</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#F78C6C;">var</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">registry</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">new</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">AutofacComponentRegistry</span><span style="color:#89DDFF;">(</span><span style="color:#A6ACCD;">builder</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#676E95;font-style:italic;">// register all components</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#F78C6C;">var</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">resolver</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">new</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">AutofacComponentResolver</span><span style="color:#89DDFF;">(</span><span style="color:#A6ACCD;">builder</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">Build</span><span style="color:#89DDFF;">());</span></span>\n<span class="line"></span></code></pre></div>', 4);
const _hoisted_5 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_5);
}
var shuttleCoreAutofac = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, shuttleCoreAutofac as default };