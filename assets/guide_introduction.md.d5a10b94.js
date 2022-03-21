import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.ed48be56.js";
var _imports_0 = "/shuttle-esb/images/endpoint.png";
const __pageData = '{"title":"The Broad Strokes","description":"","frontmatter":{},"headers":[],"relativePath":"guide/introduction.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="the-broad-strokes" tabindex="-1">The Broad Strokes <a class="header-anchor" href="#the-broad-strokes" aria-hidden="true">#</a></h1><p>All processing is performed on messages (serialized objects) that are received from a queue and then finding a message handler that can handle the type of the message (<code>instance.GetType().Name</code>). Typically messages are sent to a queue to be processed and this combination of queue and the <code>ServiceBus</code> instance that performs the processing is referred to as an <em>endpoint</em>:</p><p><img src="' + _imports_0 + '" alt=""></p><p>It is important to note that <em>not</em> every <code>ServiceBus</code> instance will process messages from an inbox queue. This happens when the instance is a producer of messages only. An example may be a <code>web-api</code> that receives integration requests that are then sent to a relevant endpoint queue as a <code>command</code> message.</p><p>Similarly, not every queue is going to be consumed by a <code>ServiceBus</code> instance. An example of this is the error queue where poison messages are routed to. These queues have to be managed out-of-band to determine the cause of the failure before moving the messages back to the inbox queue for another round of processing.</p>', 5);
const _hoisted_6 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_6);
}
var introduction = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, introduction as default };
