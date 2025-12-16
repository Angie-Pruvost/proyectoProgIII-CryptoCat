<template>
  <div class="container">

    <button class="btn back" @click="$router.back()">⬅ Volver</button>

    <h1>Vender {{ crypto.toUpperCase() }}</h1>


    <div class="card">
      <p><strong>Cantidad disponible:</strong> {{ cantidadDisponible.toFixed(6) }}</p>
      <p><strong>Precio actual:</strong> ${{ precioActual.toFixed(2) }}</p>
    </div>


    <form class="form-card" @submit.prevent="confirmarVenta">

      <label>Cantidad a vender</label>
      <input type="number" v-model.number="cantidadVenta" step="0.000001" required />

      <p v-if="total > 0">
        <strong>Total ARS:</strong> ${{ total.toFixed(2) }}
      </p>

      <button class="btn sell" type="submit">Confirmar Venta</button>
    </form>

    <p class="error" v-if="mensaje">{{ mensaje }}</p>

  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";

const route = useRoute()
const router = useRouter()

const clienteId = route.params.id
const crypto = route.params.crypto


const cantidadDisponible = ref(0)
const precioActual = ref(0)
const cantidadVenta = ref(0)
const mensaje = ref("")


const total = ref(0)


async function cargarCantidadDisponible() {
  const res = await fetch(`https://localhost:7006/api/Transacciones/cliente/${clienteId}`)
  const datos = await res.json()

  let suma = 0
  datos.forEach(tx => {
    if (tx.CryptoCode === crypto) {
      if (tx.Action === "purchase") suma += tx.CryptoAmount
      if (tx.Action === "sale") suma -= tx.CryptoAmount
    }
  })

  cantidadDisponible.value = suma
}


async function cargarPrecio() {
  const res = await fetch(`https://criptoya.com/api/bybit/${crypto}/ARS/0.1`)
  const data = await res.json()
  precioActual.value = data.totalAsk
}


async function confirmarVenta() {
  mensaje.value = ""

  if (cantidadVenta.value <= 0) {
    mensaje.value = "La cantidad debe ser mayor a 0."
    return
  }

  if (cantidadVenta.value > cantidadDisponible.value) {
    mensaje.value = "No podés vender más de lo que tenés."
    return
  }

  total.value = cantidadVenta.value * precioActual.value

  const payload = {
    ClienteId: clienteId,
    CryptoCode: crypto,
    Action: "sale",
    CryptoAmount: cantidadVenta.value,
    Money: total.value,
    Date: new Date().toISOString(),
  }

  const res = await fetch("https://localhost:7006/api/Transacciones", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload)
  })

  if (!res.ok) {
    mensaje.value = "Error registrando la venta."
    return
  }

  alert("Venta registrada correctamente ✔")

  router.push(`/cliente-dashboard/${clienteId}`)
}


onMounted(async () => {
  await cargarCantidadDisponible()
  await cargarPrecio()
})
</script>

<style scoped>
.container { max-width: 600px; margin: auto; padding: 20px; }
.card, .form-card {
  background: var(--color-card);
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 2px 8px #0003;
  margin-bottom: 20px;
}
input {
  padding: 8px;
  border: 1px solid #bbb;
  border-radius: 6px;
  width: 100%;
}
.btn {
  padding: 10px 15px;
  border-radius: 8px;
  border: none;
  cursor: pointer;
}
.btn.back { background: #ccc; }
.btn.sell { background: #ffaa00; }
.error { color: red; margin-top: 10px; }
</style>

