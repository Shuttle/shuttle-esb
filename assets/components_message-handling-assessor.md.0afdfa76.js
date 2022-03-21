import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.ed48be56.js";
const __pageData = '{"title":"Message Handling Assessor","description":"","frontmatter":{},"headers":[{"level":2,"title":"Methods","slug":"methods"},{"level":3,"title":"RegisterAssessor","slug":"registerassessor"}],"relativePath":"components/message-handling-assessor.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="message-handling-assessor" tabindex="-1">Message Handling Assessor <a class="header-anchor" href="#message-handling-assessor" aria-hidden="true">#</a></h1><p>An implementation of the <code>IMessageHandlerAssessor</code> interface is used to determine whether a message should be processed.</p><p>If you do not specify your own implementation of the <code>IMessageHandlingAssessor</code> the <code>DefaultMessageHandlingAssessor</code> will be used.</p><h2 id="methods" tabindex="-1">Methods <a class="header-anchor" href="#methods" aria-hidden="true">#</a></h2><h3 id="registerassessor" tabindex="-1">RegisterAssessor <a class="header-anchor" href="#registerassessor" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">RegisterAssessor</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">Func</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">PipelineEvent</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">bool&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">assessor</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">RegisterAssessor</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">ISpecification</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">PipelineEvent</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">specification</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Register either a function or a specification that returns <code>true</code> when the message should be processed; else <code>false</code> to ignore the message.</p><p>When the message is not processed the rest of the pipeline will still complete.</p>', 8);
const _hoisted_9 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_9);
}
var messageHandlingAssessor = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, messageHandlingAssessor as default };
