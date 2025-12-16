<template>
  <div class="container">
    <button class="btn back" @click="$router.back()">⬅ Volver</button>

    <h1>Editar Transacción #{{ id }}</h1>

    <div v-if="form" class="card">


      <label>Acción</label>
      <select v-model="form.Action">
        <option value="purchase">Compra</option>
        <option value="sale">Venta</option>
      </select>


      <label>Criptomoneda</label>
      <input v-model="form.CryptoCode" />


      <label>Cantidad</label>
      <input type="number" step="0.000001" v-model.number="form.CryptoAmount" />


      <label>Monto (ARS)</label>
      <input type="number" step="0.01" v-model.number="form.Money" />


      <label>Fecha</label>
      <input type="datetime-local" v-model="form.Date" />

      <button class="btn save" @click="guardar">Guardar cambios</button>

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

const form = ref(null);

onMounted(() => {
  cargarTransaccion();
});

async function cargarTransaccion() {
  const res = await fetch(`https://localhost:7006/api/Transacciones/${id}`);
  const data = await res.json();

  // Combierto fecha a formato editable (datetime-local)
  const fecha = new Date(data.Date);
  const local = fecha.toISOString().slice(0, 16);

  form.value = {
    Id: data.Id,
    ClienteId: data.ClienteId,
    CryptoCode: data.CryptoCode,
    CryptoAmount: data.CryptoAmount,
    Money: data.Money,
    Action: data.Action,
    Date: local
  };
}

async function guardar() {
  const payload = {
    ClienteId: form.value.ClienteId,
    CryptoCode: form.value.CryptoCode,
    CryptoAmount: form.value.CryptoAmount,
    Money: form.value.Money,
    Action: form.value.Action,
    Date: new Date(form.value.Date).toISOString(),
  };

  const res = await fetch(`https://localhost:7006/api/Transacciones/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });

  if (!res.ok) {
    alert("Error al actualizar la transacción");
    return;
  }

  alert("Transacción actualizada correctamente");
  router.push(`/transaccion/${id}`);
}
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
  display: flex;
  flex-direction: column;
  gap: 12px;
}

input, select {
  padding: 8px;
  border-radius: 6px;
  border: 1px solid #ccc;
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

.btn.save {
  margin-top: 10px;
  background: #0077ff;
  color: white;
}
</style>
