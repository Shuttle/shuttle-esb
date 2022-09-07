import { _ as _export_sfc, o as openBlock, c as createElementBlock, a as createStaticVNode } from "./app.c73b04f3.js";
const __pageData = JSON.parse('{"title":"Exception Handling","description":"","frontmatter":{},"headers":[{"level":2,"title":"Queues","slug":"queues","link":"#queues","children":[]},{"level":2,"title":"Streams","slug":"streams","link":"#streams","children":[]}],"relativePath":"guide/essentials/exception-handling.md"}');
const _sfc_main = { name: "guide/essentials/exception-handling.md" };
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="exception-handling" tabindex="-1">Exception Handling <a class="header-anchor" href="#exception-handling" aria-hidden="true">#</a></h1><p>When an exception occurs within a pipeline an <code>OnPipelineException</code> event is raised on the pipeline and any observers that have hooked onto the event will be called:</p><div class="language-c#"><button class="copy"></button><span class="lang">c#</span><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">class</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">ReceiveExceptionObserver</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">IPipelineObserver</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">OnPipelineException</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Execute</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">OnPipelineException</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">pipelineEvent</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#89DDFF;">        </span><span style="color:#676E95;">// set by calling MarkExceptionHandled</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">if</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">(</span><span style="color:#A6ACCD;">pipelineEvent</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Pipeline</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">ExceptionHandled</span><span style="color:#89DDFF;">)</span><span style="color:#A6ACCD;"> </span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">            </span><span style="color:#89DDFF;">return</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#89DDFF;">        </span><span style="color:#676E95;">// sets ExceptionHandled to true</span></span>\n<span class="line"><span style="color:#A6ACCD;">        pipelineEvent</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Pipeline</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">MarkExceptionHandled</span><span style="color:#89DDFF;">();</span><span style="color:#A6ACCD;"> </span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#89DDFF;">        </span><span style="color:#676E95;">// prevents further processing of the pipeline</span></span>\n<span class="line"><span style="color:#A6ACCD;">        pipelineEvent</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Pipeline</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">Abort</span><span style="color:#89DDFF;">();</span><span style="color:#A6ACCD;"> </span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span></code></pre></div><p>Typically you would not respond to the <code>OnPipelineException</code> event but you need to be cognizant of how it affects the message processing.</p><p>Exception handling differs between queues and streams.</p><h2 id="queues" tabindex="-1">Queues <a class="header-anchor" href="#queues" aria-hidden="true">#</a></h2><p>The more interesting bit is that the first order of business is to determine whether or not to retry the message. If the exception is of type <code>UnrecoverableHandlerException</code> the message will not be retried and will be immediately moved to the <code>ErrorQueue</code>. This means that when you are able to determine that the message will never be processed correctly, but you do not want to discard it outright, you can throw this exception and the message will be moved directly to the <code>ErrorQueue</code>. There is also an option to set the <code>IHandlerContext.ExceptionHandling</code> to <code>ExceptionHandling.Poison</code> which will also move the message directly to the <code>ErrorQueue</code>. In instances where no <code>ErrorQueue</code> has been defined the message will simply be released. In most <code>IQueue</code> implementations this will leave the message at the head of the queue and it will immediately become available for processing again. This would lead to your queue processing being blocked by this message which is, of course, less than ideal and the message should be handled or acknowledged/ignored in some way.</p><p>If the exception is not of type <code>UnrecoverableHandlerException</code> the implementation of the <code>IServiceBusPolicy</code> is used to invoke the relevant policy method. The <code>DefaultServiceBusPolicy</code> makes use of the <code>ServiceBusOptions</code> to determine the <code>MaximumFailureCount</code>. Should the number of failed messages still be within this count the message will be retried and duration to wait until the next retry is determined by using the <code>DurationToIgnoreOnFailure</code> value.</p><p>Should the message be retried the exception message is added to the <code>FailureMessages</code> collection of the <code>TransportMessage</code> and the transport message is re-enqueued, moving it to the end of the queue and the <code>IgnoreTillDate</code> property of the <code>TransportMessage</code> is also set to the duration to wait before retrying. It is also possible to set the <code>IHandlerContext.ExceptionHandling</code> to <code>ExceptionHandling.Retry</code> to explicitly retry the message when you encounter an exception.</p><p><strong>Note</strong>: you may experience queue thrashing if you do not have a <code>DeferredQueue</code> configured for your inbox and mesasges are retried. When the inbox processor dequeues the message it determines whether it can be processed by checking the <code>IgnoreTillDate</code> value. Should the <code>IgnoreTillDate</code> be in the future (deferred) it is moved to the <code>DeferredQueue</code> if there is one; else it is simply re-enqueued in the inbox which moves it to the back of the queue. Since the inbox is responsible for processing messages as quickly as possible this processing will continue until the message again becomes available for processing. Given that there is a message to process in the inbox no backing-off will occur. Please always use a deferred queue for anything that will be executed in a production environment.</p><h2 id="streams" tabindex="-1">Streams <a class="header-anchor" href="#streams" aria-hidden="true">#</a></h2><p>Exception handling for streams is mostly the same as for queues except that no retries are permitted. The reason being that a stream is not specific to any specific consumer and producing another message will immediately result in duplicate processing.</p><p>Message sent to streams also may not be deferred.</p>', 13);
const _hoisted_14 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_14);
}
const exceptionHandling = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export {
  __pageData,
  exceptionHandling as default
};
