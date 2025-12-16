<template>
  <div>

    <!-- OVERLAY -->
    <div class="overlay" v-if="props.abierto" @click="emit('toggle-menu')"></div>


    <!-- MENÚ LATERAL -->
    <nav v-if="props.abierto" class="menu abierto">

      <h2>CryptoCat</h2>

      <ul>
        <li><router-link to="/">🏠 Alta Cliente</router-link></li>
        <li><router-link to="/nueva-compra">🛒 Nueva Compra</router-link></li>
        <li><router-link to="/historial">📜 Historial</router-link></li>
        <li><router-link to="/ranking">📈 Dónde conviene comprar</router-link></li>
        <li><router-link to="/clientes">👥 Lista de Clientes</router-link></li>
      </ul>

      <button class="cerrar" @click="emit('toggle-menu')">Cerrar</button>

    </nav>
  </div>
</template>

<script setup>
import { defineProps, defineEmits } from "vue";

const props = defineProps({
  abierto: Boolean
});

const emit = defineEmits(["toggle-menu"]);
</script>

<style scoped>


/* ============================
   OVERLAY
   ============================ */
.overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.45);
  backdrop-filter: blur(2px);
  z-index: 1050;
}

/* ============================
   MENÚ LATERAL
   ============================ */
.menu {
  position: fixed;
  top: 0;
  left: -270px;

  width: 260px;
  height: 100vh;

  background: var(--color-card);
  color: var(--color-text);

  padding: 22px;
  border-right: 1px solid var(--color-border);
  box-shadow: 3px 0 12px rgba(0, 0, 0, 0.12);

  transition: left 0.28s ease-in-out;
  z-index: 1200;
}

.menu.abierto {
  left: 0;
}

/* TITULO */
.menu h2 {
  margin: 0 0 25px 0;
  font-size: 1.4rem;
  font-weight: 700;
  color: var(--color-text);
}

/* LISTA */
.menu ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.menu li {
  margin: 18px 0;
}

/* LINKS */
.menu a {
  text-decoration: none;
  font-size: 17px;

  padding: 10px 12px;
  display: block;
  border-radius: var(--radius);

  color: var(--color-text);
  transition: var(--transition);
}

/* HOVER */
.menu a:hover {
  background: var(--color-primary);
  color: white;
}

/* ACTIVO (router-link-exact-active) */
.menu a.router-link-exact-active {
  background: var(--color-primary);
  color: white;
}

/* BOTÓN CERRAR */
.cerrar {
  margin-top: 35px;
  width: 100%;

  padding: 10px;
  border: none;
  border-radius: var(--radius);

  background: var(--color-primary);
  color: white;
  cursor: pointer;

  font-weight: 600;
  transition: var(--transition);
}

.cerrar:hover {
  background: var(--color-primary-hover);
}

</style>
