import { _ as _export_sfc, c as createElementBlock, o as openBlock, b as createBaseVNode, d as createTextVNode } from "./app.2d1f51b6.js";
const __pageData = '{"title":"Implementations","description":"","frontmatter":{},"headers":[],"relativePath":"implementations/overview.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createBaseVNode("h1", {
  id: "implementations",
  tabindex: "-1"
}, [
  /* @__PURE__ */ createTextVNode("Implementations "),
  /* @__PURE__ */ createBaseVNode("a", {
    class: "header-anchor",
    href: "#implementations",
    "aria-hidden": "true"
  }, "#")
], -1);
const _hoisted_2 = /* @__PURE__ */ createBaseVNode("p", null, [
  /* @__PURE__ */ createTextVNode("These packages are those implementing the queue interfaces "),
  /* @__PURE__ */ createBaseVNode("code", null, "IQueue"),
  /* @__PURE__ */ createTextVNode(" and "),
  /* @__PURE__ */ createBaseVNode("code", null, "IQueueFactory"),
  /* @__PURE__ */ createTextVNode(", subscription manager implementations of "),
  /* @__PURE__ */ createBaseVNode("code", null, "ISubscriptionManager"),
  /* @__PURE__ */ createTextVNode(", and then the "),
  /* @__PURE__ */ createBaseVNode("code", null, "IIdempotenceService"),
  /* @__PURE__ */ createTextVNode(" implementations.")
], -1);
const _hoisted_3 = [
  _hoisted_1,
  _hoisted_2
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_3);
}
var overview = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, overview as default };
