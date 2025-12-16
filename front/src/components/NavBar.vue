<template>
  <header class="nav-wrap">
    <div class="nav-inner">

      <button class="hamburger" @click="$emit('toggle-menu')">
        ☰
      </button>

      <div class="brand">
        <h2 class="logo">CryptoCat</h2>
      </div>



      <div class="actions">
        <button class="theme-toggle" @click="toggleTheme" :aria-pressed="darkMode">
          <span v-if="!darkMode">🌙</span>
          <span v-else>☀️</span>
        </button>
      </div>
    </div>
  </header>
</template>


<script setup>
import { ref, onMounted } from 'vue'

const darkMode = ref(false)

onMounted(() => {
  darkMode.value = localStorage.getItem('darkMode') === 'true'
  aplicarTema()
})

const toggleTheme = () => {
  darkMode.value = !darkMode.value
  localStorage.setItem('darkMode', darkMode.value)
  aplicarTema()
}

const aplicarTema = () => {
  if (darkMode.value) document.documentElement.classList.add('dark')
  else document.documentElement.classList.remove('dark')
}
</script>

<style>

.nav-wrap {
  width: 100%;
  background: var(--color-card);
  border-bottom: 1px solid var(--color-border);
  box-shadow: 0 2px 5px rgba(0,0,0,0.05);
}

.nav-inner {
  max-width: 1280px;
  margin: 0 auto;
  padding: 0.75rem 1rem;

  display: flex;
  justify-content: space-between;
  align-items: center;
}

.brand .logo {
  margin: 0;
  font-size: 1.3rem;
  font-weight: 700;
  color: var(--color-text);
}

.hamburger {
  background: transparent;
  color: var(--color-text);
  border: 1px solid var(--color-border);
  padding: 6px 10px;
  font-size: 22px;
  border-radius: 6px;
  cursor: pointer;
  transition: var(--transition);
}

.hamburger:hover {
  background: var(--color-primary);
  color: white;
  border-color: var(--color-primary);
}

.nav-link {
  padding: 0.45rem 0.75rem;
  border-radius: var(--radius);
  color: var(--color-text);
  text-decoration: none;
  transition: 0.2s;
}

.nav-link.router-link-exact-active {
  background: var(--color-primary);
  color: white;
}

.nav-link:hover {
  background: var(--color-primary-hover);
  color: white;
}

.theme-toggle {
  background: var(--color-primary);
  color: white;
  border: none;
  padding: 8px 12px;
  border-radius: var(--radius);
  cursor: pointer;
  font-size: 18px;
  transition: var(--transition);
}

.theme-toggle:hover {
  background: var(--color-primary-hover);
}

@media (max-width: 640px) {
  .nav-inner {
    flex-direction: row;
    justify-content: space-between;
  }

  .brand .logo {
    font-size: 1.1rem;
  }
}
</style>
