import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.ed48be56.js";
const __pageData = '{"title":"Throttle (Windows)","description":"","frontmatter":{},"headers":[{"level":2,"title":"Registration / Activation","slug":"registration-activation"}],"relativePath":"modules/throttle.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="throttle-windows" tabindex="-1">Throttle (Windows) <a class="header-anchor" href="#throttle-windows" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Esb.Module.Throttle\n</code></pre></div><div class="info custom-block"><p class="custom-block-title">INFO</p><p>This package currently makes use of Performance Counters which are only available on Windows. In due course these will be replace with Event Counters which are cross-platform. Pull Requests are welcome if you&#39;d like to refactor in the meantime.</p></div><p>The Throttle module for Shuttle.Esb aborts pipeline processing when the CPU usage exceeds given percentage.</p><p>The module will attach the <code>ThrottleObserver</code> to the <code>OnPipelineStarting</code> event of all pipelines except the <code>StartupPipeline</code> and abort the pipeline if the SPU usage is exceeds the given percentage.</p><div class="language-xml"><pre><code><span class="line"><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">	</span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">		</span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">section</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">throttle</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.Module.Throttle.ThrottleSection, Shuttle.Esb.Module.Throttle</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">/&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">	</span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">throttle</span><span style="color:#89DDFF;"> </span></span>\n<span class="line"><span style="color:#89DDFF;">	</span><span style="color:#C792EA;">cpuUsagePercentage</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">65</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">	</span><span style="color:#C792EA;">abortCycleCount</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">5</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">	</span><span style="color:#C792EA;">performanceCounterReadInterval</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">1000</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#89DDFF;">	</span><span style="color:#C792EA;">durationToSleepOnAbort</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">1s</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><table><thead><tr><th>Attribute</th><th>Default</th><th>Description</th></tr></thead><tbody><tr><td><code>cpuUsagePercentage</code></td><td>65</td><td>The CPU usage percentage to start throttling the endpoint pipelines.</td></tr><tr><td><code>abortCycleCount</code></td><td>5</td><td>The number of times a pipeline will be aborted before running at least once.</td></tr><tr><td><code>performanceCounterReadInterval</code></td><td>1000</td><td>The number of milliseconds between reading the CPU usage performance counter. Minimun of 1000 allowed.</td></tr><tr><td><code>durationToSleepOnAbort</code></td><td>1s</td><td>The duration(s) to sleep when aborting a pipeline. Cannot be incremented for each abort.</td></tr></tbody></table><h2 id="registration-activation" tabindex="-1">Registration / Activation <a class="header-anchor" href="#registration-activation" aria-hidden="true">#</a></h2><p>The required components may be registered by calling <code>ComponentRegistryExtensions.RegisterThrottle(IComponentRegistry)</code>.</p><p>In order for the module to attach to the <code>IPipelineFactory</code> you would need to resolve it using <code>IComponentResolver.Resolve&lt;ThrottleModule&gt;()</code>.</p>', 10);
const _hoisted_11 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_11);
}
var throttle = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, throttle as default };
