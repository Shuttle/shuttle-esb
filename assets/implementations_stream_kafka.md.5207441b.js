import { _ as _export_sfc, o as openBlock, c as createElementBlock, a as createStaticVNode } from "./app.c73b04f3.js";
const __pageData = JSON.parse('{"title":"Kafka","description":"","frontmatter":{},"headers":[{"level":2,"title":"Configuration","slug":"configuration","link":"#configuration","children":[]},{"level":2,"title":"Options","slug":"options","link":"#options","children":[]}],"relativePath":"implementations/stream/kafka.md"}');
const _sfc_main = { name: "implementations/stream/kafka.md" };
const _hoisted_1 = /* @__PURE__ */ createStaticVNode('<h1 id="kafka" tabindex="-1">Kafka <a class="header-anchor" href="#kafka" aria-hidden="true">#</a></h1><div class="language-"><button class="copy"></button><span class="lang"></span><pre><code><span class="line"><span style="color:#A6ACCD;">PM&gt; Install-Package Shuttle.Esb.Kafka</span></span>\n<span class="line"><span style="color:#A6ACCD;"></span></span></code></pre></div><h2 id="configuration" tabindex="-1">Configuration <a class="header-anchor" href="#configuration" aria-hidden="true">#</a></h2><p>The URI structure is <code>kafka://configuration-name/queue-name</code>.</p><div class="language-c#"><button class="copy"></button><span class="lang">c#</span><pre><code><span class="line"><span style="color:#A6ACCD;">services</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">AddKafka</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">builder</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=&gt;</span></span>\n<span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#F78C6C;">var</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">kafkaOptions</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">new</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">KafkaOptions</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        BootstrapServers </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">localhost:9092</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        ReplicationFactor </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">1</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        NumPartitions </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">1</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        MessageSendMaxRetries </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">3</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        RetryBackoff </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> TimeSpan</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">FromSeconds</span><span style="color:#89DDFF;">(</span><span style="color:#F78C6C;">1</span><span style="color:#89DDFF;">),</span></span>\n<span class="line"><span style="color:#A6ACCD;">        EnableAutoCommit </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#FF9CAC;">false</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        EnableAutoOffsetStore </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#FF9CAC;">false</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        FlushEnqueue </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#FF9CAC;">false</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        UseCancellationToken </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#FF9CAC;">true</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        ConsumeTimeout </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> TimeSpan</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">FromSeconds</span><span style="color:#89DDFF;">(</span><span style="color:#F78C6C;">30</span><span style="color:#89DDFF;">),</span></span>\n<span class="line"><span style="color:#A6ACCD;">        OperationTimeout </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> TimeSpan</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">FromSeconds</span><span style="color:#89DDFF;">(</span><span style="color:#F78C6C;">30</span><span style="color:#89DDFF;">),</span></span>\n<span class="line"><span style="color:#A6ACCD;">        ConnectionsMaxIdle </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> TimeSpan</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Zero</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        Acks </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> Acks</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">All</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        EnableIdempotence </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> </span><span style="color:#FF9CAC;">true</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">};</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">    kafkaOptions</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">ConfigureConsumer </span><span style="color:#89DDFF;">+=</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">sender</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">args</span><span style="color:#89DDFF;">)</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        Console</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">WriteLine</span><span style="color:#89DDFF;">(</span><span style="color:#89DDFF;">$&quot;</span><span style="color:#C3E88D;">[event] : ConfigureConsumer / Uri = &#39;</span><span style="color:#89DDFF;">{((</span><span style="color:#FFCB6B;">IQueue</span><span style="color:#89DDFF;">)</span><span style="color:#A6ACCD;">sender</span><span style="color:#89DDFF;">).</span><span style="color:#A6ACCD;">Uri</span><span style="color:#89DDFF;">}</span><span style="color:#C3E88D;">&#39;</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">};</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">    kafkaOptions</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">ConfigureProducer </span><span style="color:#89DDFF;">+=</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">sender</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">args</span><span style="color:#89DDFF;">)</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=&gt;</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        Console</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">WriteLine</span><span style="color:#89DDFF;">(</span><span style="color:#89DDFF;">$&quot;</span><span style="color:#C3E88D;">[event] : ConfigureProducer / Uri = &#39;</span><span style="color:#89DDFF;">{((</span><span style="color:#FFCB6B;">IQueue</span><span style="color:#89DDFF;">)</span><span style="color:#A6ACCD;">sender</span><span style="color:#89DDFF;">).</span><span style="color:#A6ACCD;">Uri</span><span style="color:#89DDFF;">}</span><span style="color:#C3E88D;">&#39;</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">};</span></span>\n<span class="line"></span>\n<span class="line"><span style="color:#A6ACCD;">    builder</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">AddOptions</span><span style="color:#89DDFF;">(</span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">local</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">,</span><span style="color:#A6ACCD;"> kafkaOptions</span><span style="color:#89DDFF;">);</span></span>\n<span class="line"><span style="color:#89DDFF;">});</span></span>\n<span class="line"></span></code></pre></div><p>The <code>ConfigureConsumer</code> event <code>args</code> arugment exposes the <code>ConsumerConfig</code> directly for any specific options that need to be set. Similarly, the <code>ConfigureProducer</code> event <code>args</code> arugment exposes the <code>ProducerConfig</code>.</p><p>The default JSON settings structure is as follows:</p><div class="language-json"><button class="copy"></button><span class="lang">json</span><pre><code><span class="line"><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C792EA;">Shuttle</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">&quot;</span><span style="color:#FFCB6B;">Kafka</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F78C6C;">local</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">BootstrapServers</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">localhost:9092</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">ReplicationFactor</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">1</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">NumPartitions</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">1</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">MessageSendMaxRetries</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">3</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">RetryBackoff</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">00:00:01</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">EnableAutoCommit</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">false,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">EnableAutoOffsetStore</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">false,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">FlushEnqueue</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">false,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">UseCancellationToken</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">true,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">ConsumeTimeout</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">00:00:30</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">OperationTimeout</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">00:00:30</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">ConnectionsMaxIdle</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">00:00:00</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">Acks</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">&quot;</span><span style="color:#C3E88D;">All</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">,</span></span>\n<span class="line"><span style="color:#A6ACCD;">        </span><span style="color:#89DDFF;">&quot;</span><span style="color:#F07178;">EnableIdempotence</span><span style="color:#89DDFF;">&quot;</span><span style="color:#89DDFF;">:</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">true,</span></span>\n<span class="line"><span style="color:#A6ACCD;">      </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#A6ACCD;">    </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#A6ACCD;">  </span><span style="color:#89DDFF;">}</span></span>\n<span class="line"><span style="color:#89DDFF;">}</span></span>\n<span class="line"></span></code></pre></div><h2 id="options" tabindex="-1">Options <a class="header-anchor" href="#options" aria-hidden="true">#</a></h2><table><thead><tr><th>Option</th><th>Default</th><th>Description</th></tr></thead><tbody><tr><td><code>BootstrapServers</code></td><td></td><td>Initial list of brokers as a CSV list of broker host or host:port.</td></tr><tr><td><code>ReplicationFactor</code></td><td>1</td><td>The replication factor for the new topic or -1 (the default) if a replica assignment is specified instead.</td></tr><tr><td><code>NumPartitions</code></td><td>1</td><td>The number of partitions for the new topic or -1 (the default) if a replica assignment is specified.</td></tr><tr><td><code>MessageSendMaxRetries</code></td><td>3</td><td>How many times to retry sending a failing Message. <strong>Note:</strong> retrying may cause reordering unless <code>enable.idempotence</code> is set to true.</td></tr><tr><td><code>RetryBackoff</code></td><td>&quot;00:00:01&quot;</td><td>The backoff time before retrying a protocol request.</td></tr><tr><td><code>EnableAutoCommit</code></td><td>false</td><td>Automatically and periodically commit offsets in the background.</td></tr><tr><td><code>EnableAutoOffsetStore</code></td><td>false</td><td>Automatically store offset of last message provided to application.</td></tr><tr><td><code>FlushEnqueue</code></td><td>false</td><td>If <code>true</code> will call <code>Flush</code> on the producer after a message has been enqueued.</td></tr><tr><td><code>UseCancellationToken</code></td><td>true</td><td>Indicates whether a cancellation token is used for relevant methods.</td></tr><tr><td><code>ConsumeTimeout</code></td><td>&quot;00:00:30&quot;</td><td>The duration to poll for messages before returning <code>null</code>, when the cancellation token is not used.</td></tr><tr><td><code>OperationTimeout</code></td><td>&quot;00:00:30&quot;</td><td>The duration to wait for relevant <code>async</code> methods to complete before timing out.</td></tr><tr><td><code>ConnectionsMaxIdle</code></td><td>&quot;00:00:00&quot;</td><td>Close broker connections after the specified time of inactivity.</td></tr><tr><td><code>Acks</code></td><td>&quot;All&quot;</td><td>This field indicates the number of acknowledgements the leader broker must receive from ISR brokers before responding to the request.</td></tr><tr><td><code>EnableIdempotence</code></td><td>true</td><td>When set to <code>true</code>, the producer will ensure that messages are successfully produced exactly once and in the original produce order.</td></tr></tbody></table>', 10);
const _hoisted_11 = [
  _hoisted_1
];
function _sfc_render(_ctx, _cache, $props, $setup, $data, $options) {
  return openBlock(), createElementBlock("div", null, _hoisted_11);
}
const kafka = /* @__PURE__ */ _export_sfc(_sfc_main, [["render", _sfc_render]]);
export {
  __pageData,
  kafka as default
};
