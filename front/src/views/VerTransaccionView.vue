<template>
  <div class="container">

    <button class="btn back" @click="$router.back()">⬅ Volver</button>

    <h1>Transacción #{{ id }}</h1>

    <div v-if="tx" class="card">

      <p><strong>Acción:</strong>
        <span :class="tx.Action === 'purchase' ? 'green' : 'red'">
          {{ tx.Action === 'purchase' ? 'Compra' : 'Venta' }}
        </span>
      </p>

      <p><strong>Criptomoneda:</strong> {{ tx.CryptoCode.toUpperCase() }}</p>
      <p><strong>Cantidad:</strong> {{ formatCantidad(tx.CryptoAmount) }}</p>

      <p><strong>Monto ARS:</strong> {{ formatMoney(tx.Money) }}</p>

      <p><strong>Fecha:</strong> {{ formatFecha(tx.Date) }}</p>

      <hr>

      <h3>Cliente</h3>
      <p><strong>Nombre:</strong> {{ cliente.Name }}</p>
      <p><strong>Email:</strong> {{ cliente.Email }}</p>

      <div class="buttons">
        <button class="btn edit" @click="irAEditar">✏️ Editar</button>
        <button class="btn delete" @click="eliminar">🗑 Eliminar</button>
      </div>

    </div>

    <p v-else>Cargando...</p>

  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";

const route = useRoute();
const router = useRouter();

const id = route.params.id;

const tx = ref(null);
const cliente = ref({});


onMounted(async () => {
  await cargarTransaccion();
});

async function cargarTransaccion() {
  const res = await fetch(`https://localhost:7006/api/Transacciones/${id}`);
  tx.value = await res.json();


  const cli = await fetch(`https://localhost:7006/api/Clientes/${tx.value.ClienteId}`);
  cliente.value = await cli.json();
}


function irAEditar() {
  router.push(`/transaccion-editar/${id}`);
}


async function eliminar() {
  if (!confirm("¿Seguro que deseas borrar esta transacción?")) return;

  await fetch(`https://localhost:7006/api/Transacciones/${id}`, {
    method: "DELETE"
  });

  alert("Transacción eliminada");
  router.push("/historial");
}


const formatFecha = (f) => new Date(f).toLocaleString("es-AR");
const formatCantidad = (v) => parseFloat(v).toFixed(6);
const formatMoney = (v) => "$" + parseFloat(v).toFixed(2);
</script>

<style scoped>
.container {
  max-width: 650px;
  margin: auto;
  padding: 20px;
}

.card {
  background: #fff;
  padding: 20px;
  border-radius: 12px;
  margin-top: 15px;
  box-shadow: 0 2px 8px #0002;
}

.buttons {
  margin-top: 20px;
  display: flex;
  gap: 10px;
}

.btn {
  border: none;
  padding: 8px 14px;
  border-radius: 8px;
  cursor: pointer;
}

.btn.back {
  background: #ccc;
}

.btn.edit {
  background: #4da3ff;
  color: white;
}

.btn.delete {
  background: #ff4d4d;
  color: white;
}

.green { color: green; }
.red { color: red; }
</style>
