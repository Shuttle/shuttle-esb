import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode, b as createBaseVNode } from "./app.bd65a23a.js";
const __pageData = '{"title":"Shuttle.Core.Cron","description":"","frontmatter":{"title":"Shuttle.Core.Cron","layout":"api"},"headers":[{"level":2,"title":"CronExpression","slug":"cronexpression"},{"level":2,"title":"Cron Samples","slug":"cron-samples"}],"relativePath":"infrastructure/shuttle-core-cron.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode("", 15);
const _hoisted_16 = /* @__PURE__ */ createBaseVNode("p", { "day-of-week": "" }, "Format is {minute} {hour} {day-of-month} {month}", -1);
const _hoisted_17 = /* @__PURE__ */ createBaseVNode("div", { class: "language-" }, [
  /* @__PURE__ */ createBaseVNode("pre", null, [
    /* @__PURE__ */ createBaseVNode("code", null, "{minutes} : 0-59 , - * /\n{hours} :     0-23 , - * /\n{day-of-month} 1-31 , - * ? / L W\n{month} : 1-12 or JAN-DEC    , - * /\n{day-of-week} : 1-7 or SUN-SAT , - * ? / L #\n\nExamples:\n* * * * * - is every minute of every hour of every day of every month\n5,10-12,17/5 * * * * - minute 5, 10, 11, 12, and every 5th minute after that\n")
  ])
], -1);
const _hoisted_18 = [
  _hoisted_1,
  _hoisted_16,
  _hoisted_17
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_18);
}
var shuttleCoreCron = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, shuttleCoreCron as default };
