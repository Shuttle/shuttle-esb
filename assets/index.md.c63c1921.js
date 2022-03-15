import { _ as _export_sfc, o as openBlock, c as createElementBlock, b as createBaseVNode, e as createVNode, u as unref, V as VTIconShuttle, F as Fragment, a as createStaticVNode, i as isRef, r as ref, w as watch } from "./app.f52045f1.js";
var Home_vue_vue_type_style_index_0_scoped_true_lang = "";
const _hoisted_1 = { id: "hero" };
const _hoisted_2 = /* @__PURE__ */ createStaticVNode('<h1 class="tagline" data-v-31db4bdb></h1><p class="description" data-v-31db4bdb> Cross-cutting packages that are used to facilitate the development of .Net software. </p><p class="actions" data-v-31db4bdb><a class="get-started" href="/guide/introduction.html" data-v-31db4bdb> Get Started <svg class="icon" xmlns="http://www.w3.org/2000/svg" width="10" height="10" viewBox="0 0 24 24" data-v-31db4bdb><path d="M13.025 1l-2.847 2.828 6.176 6.176h-16.354v3.992h16.354l-6.176 6.176 2.847 2.828 10.975-11z" data-v-31db4bdb></path></svg></a><a class="setup" href="/guide/quick-start.html" data-v-31db4bdb>Install</a></p>', 3);
const _hoisted_5 = /* @__PURE__ */ createStaticVNode('<section id="highlights" class="vt-box-container" data-v-31db4bdb><div class="vt-box" data-v-31db4bdb><h2 data-v-31db4bdb>Framework Support</h2><div data-v-31db4bdb>Packages currently target <code data-v-31db4bdb>netstandard2.0</code> and <code data-v-31db4bdb>netstandard2.1</code> which means that they can be used with .NET Core 2.1+, .NET Framework 4.6.1+, and .NET 5.0+</div></div><div class="vt-box" data-v-31db4bdb><h2 data-v-31db4bdb>Common Patterns</h2><div data-v-31db4bdb>Many common patterns and mechanisms are provided that get you up-and-running quickly.</div></div><div class="vt-box" data-v-31db4bdb><h2 data-v-31db4bdb>Open Source</h2><div data-v-31db4bdb>These packages are free open source software licensed under the <a href="https://opensource.org/licenses/BSD-3-Clause" data-v-31db4bdb>3-Clause BSD License</a>.</div></div></section>', 1);
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
var Home = /* @__PURE__ */ _export_sfc(_sfc_main$1, [["__scopeId", "data-v-31db4bdb"]]);
const isClient = typeof window !== "undefined";
const isString = (val) => typeof val === "string";
isClient ? window : void 0;
const defaultDocument = isClient ? window.document : void 0;
isClient ? window.navigator : void 0;
isClient ? window.location : void 0;
const _global = typeof globalThis !== "undefined" ? globalThis : typeof window !== "undefined" ? window : typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : {};
const globalKey = "__vueuse_ssr_handlers__";
_global[globalKey] = _global[globalKey] || {};
_global[globalKey];
function useFavicon(newIcon = null, options = {}) {
  const {
    baseUrl = "",
    rel = "icon",
    document = defaultDocument
  } = options;
  const favicon = isRef(newIcon) ? newIcon : ref(newIcon);
  const applyIcon = (icon) => {
    document == null ? void 0 : document.head.querySelectorAll(`link[rel*="${rel}"]`).forEach((el) => el.href = `${baseUrl}${icon}`);
  };
  watch(favicon, (i, o) => {
    if (isString(i) && i !== o)
      applyIcon(i);
  }, { immediate: true });
  return favicon;
}
var _a, _b;
isClient && (window == null ? void 0 : window.navigator) && ((_a = window == null ? void 0 : window.navigator) == null ? void 0 : _a.platform) && /iP(ad|hone|od)/.test((_b = window == null ? void 0 : window.navigator) == null ? void 0 : _b.platform);
const __pageData = '{"title":"Shuttle.Core","description":"","frontmatter":{"page":true,"title":"Shuttle.Core"},"headers":[],"relativePath":"index.md"}';
const __default__ = {};
const _sfc_main = /* @__PURE__ */ Object.assign(__default__, {
  setup(__props) {
    const icon = useFavicon();
    icon.value = "/favicon.ico";
    return (_ctx, _cache) => {
      return openBlock(), createElementBlock("div", null, [
        createVNode(Home)
      ]);
    };
  }
});
export { __pageData, _sfc_main as default };
