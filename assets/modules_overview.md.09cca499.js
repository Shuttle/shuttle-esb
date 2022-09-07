import { _ as _export_sfc, o as openBlock, c as createElementBlock, a as createStaticVNode } from "./app.ab452b29.js";
const __pageData = JSON.parse('{"title":"Modules","description":"","frontmatter":{},"headers":[],"relativePath":"modules/overview.md"}');
const _sfc_main = { name: "modules/overview.md" };
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="modules" tabindex="-1">Modules <a class="header-anchor" href="#modules" aria-hidden="true">#</a></h1><p>More information on the pipelines infrastructure can be obtained in the <a href="https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-pipelines.html" target="_blank" rel="noreferrer">Shuttle.Core.Pipelines</a> documentation.</p><p>Shuttle.Esb is, therefore, also extensible via modules. These plug into a relevant pipeline to perform additional tasks within the pipeline by registering one or more observers that respond to the events raised by the pipeline.</p><p>Each pipeline has state that contains various name/value pair items. You can add state, and there are some extensions on the state that return various well-known items such as <code>GetTransportMessage()</code> that returns the <code>TransportMessage</code> on the pipeline. Prior to deserializing the transport message it will, of course, be <code>null</code>.</p><p>You can reference the <a href="https://github.com/Shuttle/Shuttle.Esb/tree/master/Shuttle.Esb/Pipeline/Pipelines" target="_blank" rel="noreferrer">Shuttle.Esb</a> code directly to get more information on the available pipelines and the events in those pipelines.</p>', 5);
const _hoisted_6 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_6);
}
const overview = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export {
  __pageData,
  overview as default
};
