<template>
  <NavBar @toggle-menu="toggleMenu" />
  <MenuLateral :abierto="menuAbierto" @toggle-menu="toggleMenu" />

  <main id="app">
    <router-view />
  </main>
</template>

<script setup>
import { ref } from 'vue'
import NavBar from './components/NavBar.vue'
import MenuLateral from './components/MenuLateral.vue'

const menuAbierto = ref(false)

const toggleMenu = () => {
  menuAbierto.value = !menuAbierto.value
}
</script>

<style scoped>
#app {
  padding: 2rem 1rem;
}

.navegacion {
  display: flex;
  justify-content: center;
  gap: 1.5rem;
  padding: 1rem;
  background: var(--color-card);
  border-radius: 12px;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.navegacion a {
  color: var(--color-text);
  font-weight: bold;
  text-decoration: none;
  padding: 6px 12px;
  border-radius: 6px;
  transition: 0.25s ease;
}

.navegacion a:hover {
  background: var(--color-btn-hover);
  color: white;
}

.router-link-exact-active {
  background: var(--color-btn);
  color: white !important;
}

.theme-toggle {
  position: fixed;
  top: 12px;
  right: 12px;
  background: var(--color-btn);
  color: white;
  border: none;
  padding: 10px 14px;
  border-radius: 8px;
  cursor: pointer;
  font-size: 1rem;
  z-index: 999;
  transition: 0.25s ease;
}

.theme-toggle:hover {
  background: var(--color-btn-hover);
}

@media (max-width: 600px) {
  .navegacion {
    flex-direction: column;
    align-items: center;
    gap: 0.8rem;
  }
}
</style>
