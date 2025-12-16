<template>
  <h1>Historial de transacciones</h1>

  <!-- SELECTOR DE CLIENTE -->
  <div class="form-group">
    <label>Seleccionar Cliente</label>
    <select v-model="selectedClientId" @change="fetchTransacciones">
      <option value="">Seleccionar</option>
      <option v-for="client in clients" :key="client.Id" :value="client.Id">
        {{ client.Name }}
      </option>
    </select>
  </div>

  <!-- BOTÓN IR AL DASHBOARD -->
  <div v-if="selectedClientId" style="margin-top: 10px;">
    <button @click="irAlDashboard">Ir al Dashboard del Cliente</button>
  </div>

  <!-- TABLA -->
  <table v-if="transacciones.length" class="transacciones-table">
    <thead>
      <tr>
        <th>Acción</th>
        <th>Criptomoneda</th>
        <th>Cantidad</th>
        <th>Monto (ARS)</th>
        <th>Fecha</th>
      </tr>
    </thead>

    <tbody>
      <tr v-for="tx in transacciones" :key="tx.Id">
        <td :class="tx.Action === 'purchase' ? 'text-green' : 'text-red'">
          {{ tx.Action === "purchase" ? "Compra" : "Venta" }}
        </td>

        <td>{{ tx.CryptoCode.toUpperCase() }}</td>
        <td>{{ formatCantidad(tx.CryptoAmount) }}</td>
        <td>{{ formatMoney(tx.Money) }}</td>
        <td>{{ formatFecha(tx.Date) }}</td>
        <td>
          <button @click="$router.push(`/transaccion/${tx.Id}`)">Ver</button>
        </td>


      </tr>
    </tbody>
  </table>

  <div v-else-if="selectedClientId">No hay transacciones para este cliente.</div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import "../styles/historial.css";
import { useRouter } from "vue-router";

const router = useRouter();

// ---- STATE ----
const clients = ref([]);
const selectedClientId = ref("");
const transacciones = ref([]);

// ---- CARGAR CLIENTES ----
onMounted(async () => {
  try {
    const response = await fetch("https://localhost:7006/api/Clientes");
    clients.value = await response.json();
  } catch (error) {
    console.error("Error cargando clientes", error);
  }
});

// ---- CARGAR TRANSACCIONES ----
const fetchTransacciones = async () => {
  if (!selectedClientId.value) return;

  try {
    const res = await fetch(
      `https://localhost:7006/api/Transacciones/cliente/${selectedClientId.value}`
    );
    transacciones.value = await res.json();
  } catch (err) {
    console.error("Error cargando transacciones", err);
  }
};

// ---- IR AL DASHBOARD ----
const irAlDashboard = () => {
  router.push(`/cliente-dashboard/${selectedClientId.value}`);
};

// ---- FORMATOS ----
const formatFecha = (f) => new Date(f).toLocaleString("es-AR");
const formatCantidad = (v) => parseFloat(v).toFixed(6);
const formatMoney = (v) => `$${parseFloat(v).toFixed(2)}`;
</script>

<style scoped>
/* tus estilos */
</style>
