import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.7c456025.js";
const __pageData = '{"title":"Shuttle.Core.TimeSpanTypeConverters","description":"","frontmatter":{"title":"Shuttle.Core.TimeSpanTypeConverters","layout":"api"},"headers":[{"level":2,"title":"StringDurationArrayConverter","slug":"stringdurationarrayconverter"}],"relativePath":"infrastructure/shuttle-core-timespantypeconverters.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="timespantypeconverters" tabindex="-1">TimeSpanTypeConverters <a class="header-anchor" href="#timespantypeconverters" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Core.TimeSpanTypeConverters\n</code></pre></div><p>Contains type converters for use with <code>TimeSpan</code>.</p><h2 id="stringdurationarrayconverter" tabindex="-1">StringDurationArrayConverter <a class="header-anchor" href="#stringdurationarrayconverter" aria-hidden="true">#</a></h2><p>The <code>StringDurationArrayConverter</code> converts from a comma-delimited string that contains durations formatted as <code>length</code> followed by <code>duration</code> as <code>ms</code> (millisecond), <code>s</code> (second), <code>m</code> (minute), <code>h</code> (hour), or <code>d</code>. It can optionally be followed by <code>*repeat</code> to repeat the duration for the specifiied number of times.</p>', 5);
const _hoisted_6 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_6);
}
var shuttleCoreTimespantypeconverters = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, shuttleCoreTimespantypeconverters as default };
