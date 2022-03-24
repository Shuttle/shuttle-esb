import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.2d1f51b6.js";
const __pageData = '{"title":"File System","description":"","frontmatter":{},"headers":[{"level":2,"title":"Configuration","slug":"configuration"}],"relativePath":"implementations/queue/filemq.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="file-system" tabindex="-1">File System <a class="header-anchor" href="#file-system" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Esb.FileMQ\n</code></pre></div><p>File-based queue implementation for use with Shuttle.Esb.</p><p>This queue implementation is not intended to be used in a production environment other than for backing up / copying queue messages.</p><h1 id="filemq" tabindex="-1">FileMQ <a class="header-anchor" href="#filemq" aria-hidden="true">#</a></h1><p>This <code>IQueue</code> implementation makes use of a folder as a queue with the messages saved as file. It is provided mainly as a backup mechanism.</p><h2 id="configuration" tabindex="-1">Configuration <a class="header-anchor" href="#configuration" aria-hidden="true">#</a></h2><p>The queue configuration is part of the specified uri, e.g.:</p><div class="language-xml"><pre><code><span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">inbox</span></span>\n<span class="line"><span style="color:#89DDFF;">      </span><span style="color:#C792EA;">workQueueUri</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">filemq://{directory-path}</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">	  .</span></span>\n<span class="line"><span style="color:#89DDFF;">	  .</span></span>\n<span class="line"><span style="color:#89DDFF;">	  .</span></span>\n<span class="line"><span style="color:#89DDFF;">    /&gt;</span></span>\n<span class="line"></span></code></pre></div>', 9);
const _hoisted_10 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_10);
}
var filemq = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, filemq as default };
