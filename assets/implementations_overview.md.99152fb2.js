import { _ as _export_sfc, o as openBlock, c as createElementBlock, a as createStaticVNode } from "./app.c73b04f3.js";
const __pageData = JSON.parse('{"title":"Implementations","description":"","frontmatter":{},"headers":[{"level":2,"title":"Queues","slug":"queues","link":"#queues","children":[]},{"level":2,"title":"Streams","slug":"streams","link":"#streams","children":[]}],"relativePath":"implementations/overview.md"}');
const _sfc_main = { name: "implementations/overview.md" };
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="implementations" tabindex="-1">Implementations <a class="header-anchor" href="#implementations" aria-hidden="true">#</a></h1><p>These packages are those implementing the queue interfaces <code>IQueue</code> and <code>IQueueFactory</code>, subscription service implementations of <code>ISubscriptionService</code>, and then the <code>IIdempotenceService</code> implementations.</p><h2 id="queues" tabindex="-1">Queues <a class="header-anchor" href="#queues" aria-hidden="true">#</a></h2><p>The convention for queue URIs is <code>scheme://configuration-name/queue-name</code> and the <code>scheme</code> represents a unique name for the <code>IQueue</code> implementation. The <code>scheme</code> and <code>configuration-name</code> (represented by the URI&#39;s <code>Host</code> property) should always be lowercase as creating a <code>new Uri(uriString)</code> forces the scheme and host to lowercase.</p><p>Each <code>configuration</code> is a named set of options and would contain all the values required to communicate with the <code>queue</code> as well as any other bits that may be of interest.</p><p>The typical JSON settings structure for a queue implementation would follow the following convetion:</p><div class="language-json"><button class="copy"></button><span class="lang">json</span><pre><code><span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C792EA;">Shuttle</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&quot;</span><span style="color:#FFCB6B;">ImplementationName</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F78C6C;">configuration-name</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">          </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">OptionA</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">value-a</span><span style="color:#89DDFF;">&quot;</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span></code></pre></div><h2 id="streams" tabindex="-1">Streams <a class="header-anchor" href="#streams" aria-hidden="true">#</a></h2><p>Stream implementations also implement the same interfaces as queues except that the <code>IQueue.IsStream</code> returns <code>true</code> which allows the service bus to handle exceptions differently.</p>', 9);
const _hoisted_10 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_10);
}
const overview = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export {
  __pageData,
  overview as default
};
