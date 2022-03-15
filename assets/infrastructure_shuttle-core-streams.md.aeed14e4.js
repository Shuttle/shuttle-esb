import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.764113ec.js";
const __pageData = '{"title":"Shuttle.Core.Streams","description":"","frontmatter":{"title":"Shuttle.Core.Streams","layout":"api"},"headers":[],"relativePath":"infrastructure/shuttle-core-streams.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="shuttle-core-streams" tabindex="-1">Shuttle.Core.Streams <a class="header-anchor" href="#shuttle-core-streams" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Core.Streams\n</code></pre></div><p>Provides <code>Stream</code> extensions.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#89DDFF;">byte[]</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">ToBytes</span><span style="color:#89DDFF;">(</span><span style="color:#C792EA;">this</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Stream</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">stream</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns the given <code>stream</code> as a <code>byte</code> array.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">Stream</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Copy</span><span style="color:#89DDFF;">(</span><span style="color:#C792EA;">this</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Stream</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">stream</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Creates a copyof the given <code>stream</code>. THe copy will be at position 0 and the source <code>stream</code> will remain at its original position.</p>', 7);
const _hoisted_8 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_8);
}
var shuttleCoreStreams = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, shuttleCoreStreams as default };
