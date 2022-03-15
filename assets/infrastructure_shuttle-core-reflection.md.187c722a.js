import { _ as _export_sfc, c as createElementBlock, o as openBlock, a as createStaticVNode } from "./app.bd65a23a.js";
const __pageData = '{"title":"Shuttle.Core.Reflection","description":"","frontmatter":{"title":"Shuttle.Core.Reflection","layout":"api"},"headers":[{"level":2,"title":"ReflectionService","slug":"reflectionservice"}],"relativePath":"infrastructure/shuttle-core-reflection.md"}';
const _sfc_main = {};
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="shuttle-core-reflection" tabindex="-1">Shuttle.Core.Reflection <a class="header-anchor" href="#shuttle-core-reflection" aria-hidden="true">#</a></h1><div class="language-"><pre><code>PM&gt; Install-Package Shuttle.Core.Reflection\n</code></pre></div><p>Provides various methods to facilitate reflection handling.</p><h2 id="reflectionservice" tabindex="-1">ReflectionService <a class="header-anchor" href="#reflectionservice" aria-hidden="true">#</a></h2><div class="language-c#"><pre><code><span class="line"><span style="color:#89DDFF;">string</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">AssemblyPath</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">Assembly</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">assembly</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns the path to the given assembly.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">Assembly</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">FindAssemblyNamed</span><span style="color:#89DDFF;">(string</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">name</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns the <code>Assembly</code> that has the requested <code>name</code>; else <code>null</code>.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">Assembly</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetAssemblies</span><span style="color:#89DDFF;">()</span></span>\n<span class="line"></span></code></pre></div><p>Returns all runtime assemblies as well as those in the <code>AppDomain.CurrentDomain.BaseDirectory</code> and <code>AppDomain.CurrentDomain.RelativeSearchPath</code>.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">Assembly</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetAssemblies</span><span style="color:#89DDFF;">(string</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">folder</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns a collection of all assemblies located in the given folder.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">Assembly</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetAssembly</span><span style="color:#89DDFF;">(string</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">assemblyPath</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns the requested assembly if found; else <code>null</code>.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">Assembly</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetMatchingAssemblies</span><span style="color:#89DDFF;">(string</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">regex</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns a collection of assemblies that have their file name matching the given <code>Regex</code> expression.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">Assembly</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetMatchingAssemblies</span><span style="color:#89DDFF;">(string</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">regex</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">string</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">folder</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns a collection of assemblies in the given <code>folder</code> that have their file name matching the given <code>regex</code> expression.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">Assembly</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetRuntimeAssemblies</span><span style="color:#89DDFF;">()</span></span>\n<span class="line"></span></code></pre></div><p>For .Net 4.6+ returns <code>AppDomain.CurrentDomain.GetAssemblies();</code>. For .Net Core 2.0+ all the <code>DependencyContext.Default.GetRuntimeAssemblyNames(RuntimeEnvironment.GetRuntimeIdentifier())</code> assembly names are resolved.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">Type</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetTypes</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">Assembly</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">assembly</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns all types in the given <code>assembly</code>.</p><div class="language-c#"><pre><code><span class="line"><span style="color:#FFCB6B;">IEnumerable</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">Type</span><span style="color:#89DDFF;">&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#82AAFF;">GetTypesAssignableTo</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">T</span><span style="color:#89DDFF;">&gt;()</span></span>\n<span class="line"><span style="color:#A6ACCD;">IEnumerable&lt;Type&gt; GetTypesAssignableTo</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">Type</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">type</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"><span style="color:#A6ACCD;">IEnumerable&lt;Type&gt; GetTypesAssignableTo&lt;T&gt;</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">Assembly</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">assembly</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"><span style="color:#A6ACCD;">IEnumerable&lt;Type&gt; GetTypesAssignableTo</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">Type</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">type</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">Assembly</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">assembly</span><span style="color:#89DDFF;">)</span></span>\n<span class="line"></span></code></pre></div><p>Returns all the types in the given <code>assembly</code> that are assignable to the <code>type</code> or <code>typeof(T)</code>; if no <code>assembly</code> is provided the all assemblies returned by <code>GetAssemblies()</code> will be scanned.</p>', 24);
const _hoisted_25 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_25);
}
var shuttleCoreReflection = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export { __pageData, shuttleCoreReflection as default };
