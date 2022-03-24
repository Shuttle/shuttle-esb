import { _ as _export_sfc, o as openBlock, c as createElementBlock, b as createBaseVNode, e as createVNode, u as unref, V as VTIconShuttle, F as Fragment, a as createStaticVNode } from "./app.2d1f51b6.js";
var Home_vue_vue_type_style_index_0_scoped_true_lang = "";
const _hoisted_1 = { id: "hero" };
const _hoisted_2 = /* @__PURE__ */ createStaticVNode('<h1 class="tagline" data-v-10814776>Autonomous Business Components</h1><p class="description" data-v-10814776> Configurable service bus that provides you with a mechanism to create cross-platform endpoints that are loosely coupled, enabling you to develop and deploy specific business functionality that can be independently versioned. </p><p class="actions" data-v-10814776><a class="why" href="/shuttle-esb/concepts/why.html" data-v-10814776>Why?</a><a class="get-started" href="/shuttle-esb/guide/introduction.html" data-v-10814776> Get Started <svg class="icon" xmlns="http://www.w3.org/2000/svg" width="10" height="10" viewBox="0 0 24 24" data-v-10814776><path d="M13.025 1l-2.847 2.828 6.176 6.176h-16.354v3.992h16.354l-6.176 6.176 2.847 2.828 10.975-11z" data-v-10814776></path></svg></a><a class="upgrade" href="/guide/quick-start.html" data-v-10814776>Upgrade to 12.0.0</a></p>', 3);
const _hoisted_5 = /* @__PURE__ */ createStaticVNode('<section id="highlights" class="vt-box-container" data-v-10814776><div class="vt-box" data-v-10814776><h2 data-v-10814776>Framework Support</h2><div data-v-10814776> Packages currently target <code data-v-10814776>netstandard2.0</code> and <code data-v-10814776>netstandard2.1</code> which means that they can be used with .NET Core 2.1+, .NET Framework 4.6.1+, and .NET 5.0+ </div></div><div class="vt-box" data-v-10814776><h2 data-v-10814776>Multiple Queues</h2><div data-v-10814776> Many popular queueing technologies are supported out-of-the-box but it is possible to implement any queue. </div></div><div class="vt-box" data-v-10814776><h2 data-v-10814776>Open Source</h2><div data-v-10814776> These packages are free open source software licensed under the <a href="https://opensource.org/licenses/BSD-3-Clause" data-v-10814776>3-Clause BSD License</a>. Pull requests are welcome. </div></div></section>', 1);
const _sfc_main$1 = {
  setup(__props) {
    return (_ctx, _cache) => {
      return openBlock(), createElementBlock(Fragment, null, [
        createBaseVNode("section", _hoisted_1, [
          createVNode(unref(VTIconShuttle), { class: "logo" }),
          _hoisted_2
        ]),
        _hoisted_5
      ], 64);
    };
  }
};
var Home = /* @__PURE__ */ _export_sfc(_sfc_main$1, [["__scopeId", "data-v-10814776"]]);
const __pageData = '{"title":"Home","description":"","frontmatter":{"page":true,"title":"Home"},"headers":[],"relativePath":"index.md"}';
const __default__ = {};
const _sfc_main = /* @__PURE__ */ Object.assign(__default__, {
  setup(__props) {
    return (_ctx, _cache) => {
      return openBlock(), createElementBlock("div", null, [
        createVNode(Home)
      ]);
    };
  }
});
export { __pageData, _sfc_main as default };
