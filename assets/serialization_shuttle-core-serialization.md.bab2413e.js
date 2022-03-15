import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.bd65a23a.js";
const __pageData = '{"title":"Shuttle.Core.Serialization","description":"","frontmatter":{},"headers":[{"level":2,"title":"Methods","slug":"methods"},{"level":3,"title":"Serialize","slug":"serialize"},{"level":3,"title":"Deserialize","slug":"deserialize"},{"level":2,"title":"AddSerializerType","slug":"addserializertype"}],"relativePath":"serialization/shuttle-core-serialization.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="shuttle-core-serialization" tabindex="-1">Shuttle.Core.Serialization <a class="header-anchor" href="#shuttle-core-serialization" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Core.Serialization\n</code></pre></div><p>An implementation of the <code>ISerializer</code> interface is used to serialize objects into a <code>Stream</code>.</p><p>The <code>DefaultSerializer</code> makes use of the standard .NET xml serialization functionality.</p><h2 id="methods" tabindex="-1">Methods <a class="header-anchor" href="#methods" aria-hidden="true">#</a></h2><h3 id="serialize" tabindex="-1">Serialize <a class="header-anchor" href="#serialize" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">Stream</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Serialize</span><span style="color:#89DDFF;">(object</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">message</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Returns the message <code>object</code> as a <code>Stream</code>.</p><h3 id="deserialize" tabindex="-1">Deserialize <a class="header-anchor" href="#deserialize" aria-hidden="true">#</a></h3><div class="language-c#"><pre><code><span class="line"><span style="color:#89DDFF;">object</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">Deserialize</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">Type</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">type</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Stream</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">stream</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Deserializes the <code>Stream</code> into an <code>obejct</code> of the given type.</p><h1 id="iserializerroottype" tabindex="-1">ISerializerRootType <a class="header-anchor" href="#iserializerroottype" aria-hidden="true">#</a></h1><p>The <code>ISerializerRootType</code> interface is an optional interface that serializer implementations can use that allows the developer to specify explicit object types contained within a root type.</p><p>The <code>DefaultSerializer</code> implements this interface and it is recommended that you explicitly register types with the same name, but in different namespaes, that will be serialized within the same root type to avoid any conflicts later down the line.</p><p>For instance, the following two types will cause issues when used in the root <code>Complex</code> type as they both serialize to the same name and the .Net serializer cannot seem to distinguish the difference:</p><div class="language-c#"><pre><code><span class="line"><span style="color:#F78C6C;">namespace</span><span style="color:#A6ACCD;"> Serializer</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">v1</span></span>\n<span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">class</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">MovedEvent</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">string</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Where</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">get</span><span style="color:#89DDFF;">;</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">set</span><span style="color:#89DDFF;">;</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">}</span><span style="color:#A6ACCD;"> </span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#F78C6C;">namespace</span><span style="color:#A6ACCD;"> Serializer</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">v2</span></span>\n<span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">class</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">MovedEvent</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">string</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Where</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">get</span><span style="color:#89DDFF;">;</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">set</span><span style="color:#89DDFF;">;</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">}</span><span style="color:#A6ACCD;"> </span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#F78C6C;">namespace</span><span style="color:#A6ACCD;"> Serializer</span></span>\n<span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">class</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Complex</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> v1.MovedEvent { get</span><span style="color:#89DDFF;">;</span><span style="color:#A6ACCD;"> set</span><span style="color:#89DDFF;">;</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#C792EA;">public</span><span style="color:#A6ACCD;"> v2.MovedEvent { get</span><span style="color:#89DDFF;">;</span><span style="color:#A6ACCD;"> set</span><span style="color:#89DDFF;">;</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#A6ACCD;">    }</span></span>\n<span class="line"><span style="color:#A6ACCD;">}</span></span>\n<span class="line"></span></code></pre></div><p>By explicitly specifying the types the <code>DefaultSerializer</code> will add a namespace that will cause the types to be correctly identified.</p><h2 id="addserializertype" tabindex="-1">AddSerializerType <a class="header-anchor" href="#addserializertype" aria-hidden="true">#</a></h2><div class="language-c#"><pre><code><span class="line"><span style="color:#89DDFF;">void</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">AddSerializerType</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">Type</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">root</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Type</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">contained</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"></span></code></pre></div><p>Specify the <code>contained</code> type that is used within the <code>root</code> type somewhere.</p>', 20);
const _hoisted_21 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_21);
}
var shuttleCoreSerialization = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, shuttleCoreSerialization as default };
