<script setup>
import { VTIconShuttle } from 'shuttle-theme'
</script>
<template>
  <section id="hero">
    <VTIconShuttle class="logo" />
    <h1 class="tagline">Autonomous Business Components</h1>
    <p class="description">
      Configurable service bus that provides you with a mechanism to create
      cross-platform endpoints that are loosely coupled, enabling you to develop
      and deploy specific business functionality that can be independently
      versioned.
    </p>
    <p class="actions">
      <a class="why" href="/shuttle-esb/concepts/why.html">Why?</a>
      <a class="get-started" href="/shuttle-esb/guide/introduction.html">
        Get Started
        <svg
          class="icon"
          xmlns="http://www.w3.org/2000/svg"
          width="10"
          height="10"
          viewBox="0 0 24 24"
        >
          <path
            d="M13.025 1l-2.847 2.828 6.176 6.176h-16.354v3.992h16.354l-6.176 6.176 2.847 2.828 10.975-11z"
          />
        </svg>
      </a>
      <a class="upgrade" href="/guide/upgrade-12.0.0.html">Upgrade to 12.0.0</a>
    </p>
  </section>

  <section id="highlights" class="vt-box-container">
    <div class="vt-box">
      <h2>Framework Support</h2>
      <div>
        Packages currently target <code>netstandard2.0</code> and
        <code>netstandard2.1</code> which means that they can be used with .NET
        Core 2.1+, .NET Framework 4.6.1+, and .NET 5.0+
      </div>
    </div>
    <div class="vt-box">
      <h2>Multiple Queues</h2>
      <div>
        Many popular queueing technologies are supported out-of-the-box but it
        is possible to implement any queue.
      </div>
    </div>
    <div class="vt-box">
      <h2>Open Source</h2>
      <div>
        These packages are free open source software licensed under the
        <a href="https://opensource.org/licenses/BSD-3-Clause"
          >3-Clause BSD License</a
        >. Pull requests are welcome.
      </div>
    </div>
  </section>
</template>

<style scoped>
a {
  color: var(--vt-c-orange-dark);
}

a:hover {
  color: var(--vt-c-orange);
}

.logo {
  height: 15em;
  width: 15em;
  margin: auto;
}
section {
  padding: 42px 32px;
}

#hero {
  padding: 32px 32px;
  text-align: center;
}

.tagline {
  font-size: 76px;
  line-height: 1.25;
  font-weight: 900;
  letter-spacing: -1.5px;
  max-width: 960px;
  margin: 0px auto;
}

html:not(.dark) .accent,
.dark .tagline {
  background: -webkit-linear-gradient(
    315deg,
    hsl(31, 75%, 52%) 25%,
    hsl(45, 69%, 44%)
  );
  background-clip: text;
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.description {
  max-width: 960px;
  line-height: 1.5;
  color: var(--vt-c-text-2);
  transition: color 0.5s;
  font-size: 22px;
  margin: 24px auto 40px;
}

.actions a {
  font-size: 16px;
  display: inline-block;
  background-color: var(--vt-c-bg-mute);
  padding: 8px 18px;
  font-weight: 500;
  border-radius: 8px;
  transition: background-color 0.5s, color 0.5s;
}

.actions .why {
  background-color: var(--vt-c-indigo);
  color: var(--vt-c-gray-light-4);
  margin-right: 18px;
}

.actions .why:hover {
  background-color: var(--vt-c-indigo-soft);
  color: var(--vt-c-gray-light-1);
}

.actions .get-started {
  font-weight: 600;
  background-color: var(--vt-c-orange);
  color: #fff;
  margin-right: 18px;
}

.actions .icon {
  display: inline;
  position: relative;
  top: -1px;
  margin-left: 2px;
  fill: currentColor;
  transition: transform 0.2s;
}

.dark .actions .get-started {
  color: var(--vt-c-indigo);
}

.actions .get-started:hover {
  background-color: var(--vt-c-orange-dark);
  transition-duration: 0.2s;
}

.actions .get-started:hover .icon {
  transform: translateX(2px);
}

.dark .actions .get-started:hover {
  background-color: var(--vt-c-orange-light);
}

.actions .upgrade {
  color: var(--vt-c-text-code);
}

.actions .upgrade:hover {
  background-color: var(--vt-c-gray-light-4);
  transition-duration: 0.2s;
}

.dark .actions .upgrade:hover {
  background-color: var(--vt-c-gray-dark-3);
}

#special-sponsor {
  border-top: 1px solid var(--vt-c-divider-light);
  border-bottom: 1px solid var(--vt-c-divider-light);
  padding: 12px 24px;
  text-align: center;
}

#special-sponsor span {
  color: var(--vt-c-text-2);
  font-weight: 500;
  font-size: 13px;
  vertical-align: middle;
  margin: 0 24px;
}

#special-sponsor img {
  display: inline-block;
  vertical-align: middle;
  height: 36px;
}

.dark #special-sponsor img {
  filter: grayscale(1) invert(1);
}

#highlights {
  max-width: 960px;
  margin: 0px auto;
  color: var(--vt-c-text-2);
}

#highlights h2 {
  font-weight: 600;
  font-size: 20px;
  letter-spacing: -0.4px;
  color: var(--vt-c-text-1);
  transition: color 0.5s;
  margin-bottom: 0.75em;
}

#highlights p {
  font-weight: 400;
  font-size: 15px;
}

#highlights .vt-box {
  background-color: transparent;
}

#sponsors {
  max-width: 900px;
  margin: 0px auto;
}

#sponsors h2 {
  font-size: 20px;
  font-weight: 600;
  margin-bottom: 1em;
}

#sponsors .sponsor-container {
  margin-bottom: 3em;
}

@media (max-width: 960px) {
  .tagline {
    font-size: 64px;
    letter-spacing: -0.5px;
  }
  .description {
    font-size: 18px;
    margin-bottom: 48px;
  }
}

@media (max-width: 768px) {
  .tagline {
    font-size: 48px;
    letter-spacing: -0.5px;
  }
}

@media (max-width: 576px) {
  #hero {
    padding: 64px 32px;
  }
  .description {
    font-size: 16px;
    margin: 18px 0 30px;
  }
  #special-sponsor img {
    display: block;
    margin: 2px auto 1px;
  }
  #highlights h3 {
    margin-bottom: 0.6em;
  }
  #highlights .vt-box {
    padding: 20px 36px;
  }
}

@media (max-width: 370px) {
  .tagline {
    font-size: 36px;
  }
}
</style>
