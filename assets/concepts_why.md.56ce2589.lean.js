import { _ as _export_sfc, c as createElementBlock, b as createBaseVNode, d as createTextVNode, t as toDisplayString, a as createStaticVNode, o as openBlock } from "./app.ed48be56.js";
const __pageData = '{"title":"Why use a service bus?","description":"","frontmatter":{},"headers":[{"level":2,"title":"Core","slug":"core"},{"level":2,"title":"Messages","slug":"messages"},{"level":2,"title":"Queues","slug":"queues"},{"level":2,"title":"Service bus","slug":"service-bus"},{"level":2,"title":"Message Types","slug":"message-types"},{"level":3,"title":"Command message","slug":"command-message"},{"level":3,"title":"Starting a process","slug":"starting-a-process"},{"level":3,"title":"Lower-level functions (RPC)","slug":"lower-level-functions-rpc"},{"level":3,"title":"Event message","slug":"event-message"},{"level":3,"title":"Document message","slug":"document-message"},{"level":2,"title":"Coupling","slug":"coupling"},{"level":3,"title":"Behavioural coupling","slug":"behavioural-coupling"},{"level":3,"title":"Temporal coupling","slug":"temporal-coupling"}],"relativePath":"concepts/why.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode("", 22);
const _hoisted_23 = /* @__PURE__ */ createTextVNode("A service bus instance is created and started on application startup and disposed on exit. A service bus can be hosted in any type of application but the most typical scenario is to host them as services. Although you ");
const _hoisted_24 = /* @__PURE__ */ createBaseVNode("em", null, "can", -1);
const _hoisted_25 = /* @__PURE__ */ createStaticVNode("", 32);
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, [
    _hoisted_1,
    createBaseVNode("p", null, [
      _hoisted_23,
      _hoisted_24,
      createTextVNode(" write your own service to host your service bus it is not a requirement since you may want to make use of the [generic service host](" + toDisplayString("/generic-host/index.html" | _ctx.relative_url) + ").", 1)
    ]),
    _hoisted_25
  ]);
}
var why = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, why as default };
