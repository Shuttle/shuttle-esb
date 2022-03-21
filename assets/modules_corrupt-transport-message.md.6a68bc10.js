import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.ed48be56.js";
const __pageData = '{"title":"Corrupt Transport Message","description":"","frontmatter":{},"headers":[{"level":2,"title":"Registration / Activation","slug":"registration-activation"}],"relativePath":"modules/corrupt-transport-message.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="corrupt-transport-message" tabindex="-1">Corrupt Transport Message <a class="header-anchor" href="#corrupt-transport-message" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Esb.Module.CorruptTransportMessage\n</code></pre></div><p>The CorruptTransportMessage module for Shuttle.Esb writes any transport messages that fail to deserialize to disk.</p><p>It will log any transport messages that fail deserailization via the <code>ServiceBusEvents.TransportMessageDeserializationException</code> event to a folder as specified in the configuration:</p><div class="language-xml"><pre><code><span class="line"><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">	</span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">		</span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">section</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">name</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">corruptTransportMessage</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">type</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">Shuttle.Esb.Module.CorruptTransportMessage.CorruptTransportMessageSection, Shuttle.Esb.Module.CorruptTransportMessage</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">/&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">	</span><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configSections</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">&lt;</span><span style="color:#F07178;">corruptTransportMessage</span><span style="color:#89DDFF;"> </span><span style="color:#C792EA;">folder</span><span style="color:#89DDFF;">=</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">.\\corrupt-transport-messages</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;"> /&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">&lt;/</span><span style="color:#F07178;">configuration</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"></span></code></pre></div><p>The default value for the <code>folder</code> attribute is <code>.\\corrupt-transport-messages</code>.</p><h2 id="registration-activation" tabindex="-1">Registration / Activation <a class="header-anchor" href="#registration-activation" aria-hidden="true">#</a></h2><p>The required components may be registered by calling <code>ComponentRegistryExtensions.RegisterCorruptTransportMessage(IComponentRegistry)</code>.</p><p>In order for the module to attach to the <code>IPipelineFactory</code> you would need to resolve it using <code>IComponentResolver.Resolve&lt;CorruptTransportMessageModule&gt;()</code>.</p>', 9);
const _hoisted_10 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_10);
}
var corruptTransportMessage = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, corruptTransportMessage as default };
