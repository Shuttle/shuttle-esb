import { _ as _export_sfc, o as openBlock, c as createElementBlock, a as createStaticVNode } from "./app.c73b04f3.js";
const __pageData = JSON.parse('{"title":"Throttle","description":"","frontmatter":{},"headers":[{"level":2,"title":"Configuration","slug":"configuration","link":"#configuration","children":[]},{"level":2,"title":"Options","slug":"options","link":"#options","children":[]}],"relativePath":"modules/throttle.md"}');
const _sfc_main = { name: "modules/throttle.md" };
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="throttle" tabindex="-1">Throttle <a class="header-anchor" href="#throttle" aria-hidden="true">#</a></h1><div class="language-"><button class="copy"></button><span class="lang"></span><pre><code><span class="line"><span style="color:#A6ACCD;">PM&gt; Install-Package Shuttle.Esb.Module.Throttle</span></span>\n<span class="line"><span style="color:#A6ACCD;"></span></span></code></pre></div><p>The Throttle module for Shuttle.Esb aborts pipeline processing when the CPU usage exceeds given percentage.</p><p>The module will attach the <code>ThrottleObserver</code> to the <code>OnPipelineStarting</code> event of the <code>InboxMessagePipeline</code> and abort the pipeline if the CPU usage exceeds the given percentage.</p><h2 id="configuration" tabindex="-1">Configuration <a class="header-anchor" href="#configuration" aria-hidden="true">#</a></h2><div class="language-c#"><button class="copy"></button><span class="lang">c#</span><pre><code><span class="line"><span style="color:#A6ACCD;">services</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">AddThrottleModule</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">builder</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=&gt;</span><span style="color:#A6ACCD;"> </span></span>\n<span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">	builder</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Options</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">CpuUsagePercentage </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">65</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"><span style="color:#A6ACCD;">	builder</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Options</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">AbortCycleCount </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">5</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"><span style="color:#A6ACCD;">	builder</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Options</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">DurationToSleepOnAbort </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">new</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">List</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">TimeSpan</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span><span style="color:#A6ACCD;"> TimeSpan</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">FromSeconds</span><span style="color:#89DDFF;">(</span><span style="color:#F78C6C;">1</span><span style="color:#89DDFF;">)</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">};</span></span>\n<span class="line"><span style="color:#89DDFF;">})</span></span>\n<span class="line"></span></code></pre></div><h2 id="options" tabindex="-1">Options <a class="header-anchor" href="#options" aria-hidden="true">#</a></h2><table><thead><tr><th>Option</th><th>Default</th><th>Description</th></tr></thead><tbody><tr><td><code>CpuUsagePercentage</code></td><td>65</td><td>The CPU usage percentage threshold to start throttling.</td></tr><tr><td><code>AbortCycleCount</code></td><td>5</td><td>The number of times a pipeline will be aborted before running at least once.</td></tr><tr><td><code>DurationToSleepOnAbort</code></td><td>&quot;00:00:01&quot;</td><td>The duration(s) to sleep when aborting a pipeline. Can be incremented for each abort.</td></tr></tbody></table>', 8);
const _hoisted_9 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_9);
}
const throttle = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export {
  __pageData,
  throttle as default
};
