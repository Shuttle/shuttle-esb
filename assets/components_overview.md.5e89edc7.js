import { _ as _export_sfc, o as openBlock, c as createElementBlock, b as createBaseVNode, d as createTextVNode } from "./app.c73b04f3.js";
const __pageData = JSON.parse('{"title":"Components","description":"","frontmatter":{},"headers":[],"relativePath":"components/overview.md"}');
const _sfc_main = { name: "components/overview.md" };
const _hoisted_1 = /* @__PURE__ */ createBaseVNode("h1", {
  id: "components",
  tabindex: "-1"
}, [
  /* @__PURE__ */ createTextVNode("Components "),
  /* @__PURE__ */ createBaseVNode("a", {
    class: "header-anchor",
    href: "#components",
    "aria-hidden": "true"
  }, "#")
], -1);
const _hoisted_2 = /* @__PURE__ */ createBaseVNode("p", null, "Shuttle.Esb is highly configurable and all the major components have been abstracted behind interfaces. As such it is possible to replace any of the components withh custom implementations if required.", -1);
const _hoisted_3 = /* @__PURE__ */ createBaseVNode("p", null, [
  /* @__PURE__ */ createTextVNode("In order to replace a component you would need to register it before invoking the "),
  /* @__PURE__ */ createBaseVNode("code", null, "services.AddServiceBus()"),
  /* @__PURE__ */ createTextVNode(" method since the relevant components are registered using the "),
  /* @__PURE__ */ createBaseVNode("code", null, "service.Try*()"),
  /* @__PURE__ */ createTextVNode(" methods which would ignore a registration if it already exists. There are some components that are not optionally registered and can therefore not be replaced. If you find that you need to do so please log an issue in order to investigate the use-case.")
], -1);
const _hoisted_4 = [
  _hoisted_1,
  _hoisted_2,
  _hoisted_3
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_4);
}
const overview = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export {
  __pageData,
  overview as default
};
