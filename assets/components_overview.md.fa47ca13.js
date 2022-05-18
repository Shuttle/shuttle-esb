import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.c706ac22.js";
const __pageData = '{"title":"Components","description":"","frontmatter":{},"headers":[],"relativePath":"components/overview.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="components" tabindex="-1">Components <a class="header-anchor" href="#components" aria-hidden="true">#</a></h1><p>Shuttle.Esb is highly configurable and all the major components have been abstracted behind interfaces. As such it is possible to replace any of the components withh custom implementations if required.</p><p>In order to replace a component you would need to register it before invoking the <code>Shuttle.Esb.ComponentRegistryExtensions.RegisterServiceBus()</code> method since the <code>RegisterServiceBus()</code> method makes use of the <code>IComponentRegistry.AttemptRegister()</code> method which would ignore a registration if it already exists.</p><p>You can also register message handlers separately from any <code>Assembly</code> using:</p><div class="language-c#"><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#C792EA;">static</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">RegisterMessageHandlers</span><span style="color:#89DDFF;">(</span><span style="color:#C792EA;">this</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">IComponentRegistry</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">registry</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Assembly</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">assembly</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div>', 5);
const _hoisted_6 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_6);
}
var overview = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, overview as default };
