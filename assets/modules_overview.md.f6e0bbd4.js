import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.ed48be56.js";
const __pageData = '{"title":"Modules","description":"","frontmatter":{},"headers":[{"level":2,"title":"Implementation","slug":"implementation"},{"level":2,"title":"Pipelines","slug":"pipelines"}],"relativePath":"modules/overview.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="modules" tabindex="-1">Modules <a class="header-anchor" href="#modules" aria-hidden="true">#</a></h1><p>Shuttle.Esb is extensible via modules. These plug into a relevant pipeline to perform additional tasks within the pipeline by registering one or more observers that respond to the events raised by the pipeline.</p><h2 id="implementation" tabindex="-1">Implementation <a class="header-anchor" href="#implementation" aria-hidden="true">#</a></h2><p>A module is an arbitrary class that should use the <code>IPipelineFactory</code> implementation to hook onto the relevant pipeline.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">class</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">LogMessageOwnerModule</span></span>\n<span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#C792EA;">private</span><span style="color:#A6ACCD;"> </span><span style="color:#C792EA;">readonly</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">LogMessageOwnerObserver</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">_logMessageOwnerObserver</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#C792EA;">private</span><span style="color:#A6ACCD;"> </span><span style="color:#C792EA;">readonly</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">string</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">_inboxMessagePipelineName</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">typeof</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">InboxMessagePipeline</span><span style="color:#89DDFF;">).</span><span style="color:#A6ACCD;">FullName</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">LogMessageOwnerModule</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">IPipelineFactory</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">pipelineFactory</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">LogMessageOwnerObserver</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">logMessageOwnerObserver</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        Guard</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">AgainstNull</span><span style="color:#89DDFF;">(</span><span style="color:#A6ACCD;">pipelineFactory</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">pipelineFactory</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"><span style="color:#A6ACCD;">        Guard</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">AgainstNull</span><span style="color:#89DDFF;">(</span><span style="color:#A6ACCD;">logMessageOwnerObserver</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">logMessageOwnerObserver</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span></span>\n<span class="line"><span style="color:#A6ACCD;">        _logMessageOwnerObserver </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> logMessageOwnerObserver</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">        pipelineFactory</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">PipelineCreated </span><span style="color:#89DDFF;">+=</span><span style="color:#A6ACCD;"> PipelineCreated</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#C792EA;">private</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">PipelineCreated</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">sender</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">PipelineEventArgs</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">e</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;font-style:italic;">if</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">(!</span><span style="color:#A6ACCD;">e</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Pipeline</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">GetType</span><span style="color:#89DDFF;">().</span><span style="color:#A6ACCD;">FullName</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">Equals</span><span style="color:#89DDFF;">(</span><span style="color:#A6ACCD;">_inboxMessagePipelineName</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> StringComparison</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">InvariantCultureIgnoreCase</span><span style="color:#89DDFF;">))</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">            </span><span style="color:#89DDFF;font-style:italic;">return</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">        e</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Pipeline</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">RegisterObserver</span><span style="color:#89DDFF;">(</span><span style="color:#A6ACCD;">_logMessageOwnerObserver</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span></code></pre></div><p>You may be wondering where the <code>LogMessageOwnerObserver</code> instance would come from. The <code>Shuttle.Esb.ComponentRegistryExtensions.RegisterServiceBus()</code> method registers, in the <code>IComponentRegistry</code> instance, all observer types that it finds in the application. However, to be sure that the relevant dependencies for you modules are registered you could use the <code>IComponentRegistry</code> instance to directly register them:</p><div class="language-c#"><pre><code><span class="line"><span style="color:#A6ACCD;">registry</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">AttemptRegister</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">LogMessageOwnerModule</span><span style="color:#89DDFF;">&gt;();</span></span>\n<span class="line"><span style="color:#A6ACCD;">registry</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">AttemptRegister</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">LogMessageOwnerObserver</span><span style="color:#89DDFF;">&gt;();</span></span>\n<span class="line"></span></code></pre></div><p>Here we have created a new module that registers the <code>LogMessageOwnerObserver</code> for each newly created <code>InboxMessagePipeline</code>. Since a pipeline simply raises <code>PipelineEvent</code> instances the observer will need to listen out for the relevant events. We will log the message owner after the transport message has been deserialized:</p><div class="language-c#"><pre><code><span class="line"><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">class</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">LogMessageOwnerObserver</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">IPipelineObserver</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">OnAfterDeserializeTransportMessage</span><span style="color:#89DDFF;">&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Execute</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">OnDeserializeTransportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">pipelineEvent</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#F78C6C;">var</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">state</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> pipelineEvent</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Pipeline</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">State</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#F78C6C;">var</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">transportMessage</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> state</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">GetTransportMessage</span><span style="color:#89DDFF;">();</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;font-style:italic;">if</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">(</span><span style="color:#A6ACCD;">transportMessage </span><span style="color:#89DDFF;">==</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">null)</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">            </span><span style="color:#89DDFF;font-style:italic;">return</span><span style="color:#89DDFF;">;</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span></span>\n<span class="line"><span style="color:#A6ACCD;">        Console</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">Log</span><span style="color:#89DDFF;">(</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">This transport message belongs to &#39;{0}&#39;.</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> transportMessage</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">PrincipalIdentityName</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span></code></pre></div><p>Each pipeline has a state that contains various items. You can add state and there are some extensions on the state that return various well-known items such as <code>GetTransportMessage()</code> that returns the <code>TransportMessage</code> on the pipeline. Prior to deserializing the transport message it will, of course, be <code>null</code>.</p><p>Pipelines are re-used so they are created as needed and returned to a pool. Should a pipeline be retrieved from the pool it will be re-initialized so that the previous state is removed.</p><h2 id="pipelines" tabindex="-1">Pipelines <a class="header-anchor" href="#pipelines" aria-hidden="true">#</a></h2><p>You can reference the <a href="https://github.com/Shuttle/Shuttle.Esb/tree/master/Shuttle.Esb/Pipeline/Pipelines" target="_blank" rel="noopener noreferrer">Shuttle.Esb</a> code directly to get more information on the available pipelines and the events in those pipelines.</p><p>More information on the pipelines infrastructure can be obtained in the <a href="https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-pipelines.html" target="_blank" rel="noopener noreferrer">Shuttle.Core.Pipelines</a> documentation.</p>', 14);
const _hoisted_15 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_15);
}
var overview = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, overview as default };
