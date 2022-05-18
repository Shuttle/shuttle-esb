import { _ as _export_sfc, o as openBlock, c as createElementBlock, b as createBaseVNode, e as createVNode, u as unref, V as VTIconShuttle, f as VTIconDiscord, F as Fragment, a as createStaticVNode, d as createTextVNode } from "./app.c706ac22.js";
var Home_vue_vue_type_style_index_0_scoped_true_lang = "";
const _hoisted_1 = { id: "hero" };
const _hoisted_2 = /* @__PURE__ */ createStaticVNode('<h1 class="tagline" data-v-36068aaf>Autonomous Business Components</h1><p class="description" data-v-36068aaf> Configurable service bus that provides you with a mechanism to create cross-platform endpoints that are loosely coupled, enabling you to develop and deploy specific business functionality that can be independently versioned. </p><p class="actions" data-v-36068aaf><a class="why" href="/shuttle-esb/concepts/why.html" data-v-36068aaf>Why?</a><a class="get-started" href="/shuttle-esb/guide/introduction.html" data-v-36068aaf> Get Started <svg class="icon" xmlns="http://www.w3.org/2000/svg" width="10" height="10" viewBox="0 0 24 24" data-v-36068aaf><path d="M13.025 1l-2.847 2.828 6.176 6.176h-16.354v3.992h16.354l-6.176 6.176 2.847 2.828 10.975-11z" data-v-36068aaf></path></svg></a><a class="upgrade" href="/shuttle-esb/guide/upgrade-12.0.0.html" data-v-36068aaf>Upgrade</a></p>', 3);
const _hoisted_5 = {
  href: "https://discord.gg/Q2yEsfht6f",
  target: "_blank"
};
const _hoisted_6 = { class: "discord-link" };
const _hoisted_7 = /* @__PURE__ */ createTextVNode("Join our Discord channel ");
const _hoisted_8 = /* @__PURE__ */ createStaticVNode('<section id="highlights" class="vt-box-container" data-v-36068aaf><div class="vt-box" data-v-36068aaf><h2 data-v-36068aaf>Framework Support</h2><div data-v-36068aaf> Packages currently target <code data-v-36068aaf>netstandard2.0</code> and <code data-v-36068aaf>netstandard2.1</code> which means that they can be used with .NET Core 2.1+, .NET Framework 4.6.1+, and .NET 5.0+ </div></div><div class="vt-box" data-v-36068aaf><h2 data-v-36068aaf>Multiple Queues</h2><div data-v-36068aaf> Many popular queueing technologies are supported out-of-the-box but it is possible to implement any queue. </div></div><div class="vt-box" data-v-36068aaf><h2 data-v-36068aaf>Open Source</h2><div data-v-36068aaf> These packages are free open source software licensed under the <a href="https://opensource.org/licenses/BSD-3-Clause" data-v-36068aaf>3-Clause BSD License</a>. Pull requests are welcome. </div></div></section>', 1);
const _sfc_main$1 = {
  setup(__props) {
    return (_ctx, _cache) => {
      return openBlock(), createElementBlock(Fragment, null, [
        createBaseVNode("section", _hoisted_1, [
          createVNode(unref(VTIconShuttle), { class: "logo" }),
          _hoisted_2,
          createBaseVNode("p", null, [
            createBaseVNode("a", _hoisted_5, [
              createBaseVNode("div", _hoisted_6, [
                createVNode(unref(VTIconDiscord), { class: "discord-logo" }),
                _hoisted_7
              ])
            ])
          ])
        ]),
        _hoisted_8
      ], 64);
    };
  }
};
var Home = /* @__PURE__ */ _export_sfc(_sfc_main$1, [["__scopeId", "data-v-36068aaf"]]);
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
