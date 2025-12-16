<template>
  <div class="container">
    <button class="btn back" @click="$router.back()">⬅ Volver</button>
    <h1>Histórico de {{ crypto.toUpperCase() }}</h1>

    <canvas id="chart"></canvas>
  </div>
</template>

<script setup>
import { onMounted } from "vue"
import { useRoute } from "vue-router"
import Chart from "chart.js/auto"

const route = useRoute()
const crypto = route.params.crypto

async function cargarDatos() {
  const res = await fetch(`https://criptoya.com/api/${crypto}/ars/30`)
  const data = await res.json()

  const labels = Object.keys(data).map(k => new Date(data[k].timestamp * 1000).toLocaleDateString())
  const precios = Object.keys(data).map(k => data[k].price)

  const ctx = document.getElementById("chart")

  new Chart(ctx, {
    type: "line",
    data: {
      labels,
      datasets: [{
        label: `Precio de ${crypto.toUpperCase()}`,
        data: precios,
        fill: false,
        borderWidth: 2
      }]
    }
  })
}

onMounted(() => cargarDatos())
</script>

<style scoped>
.container { max-width: 900px; margin: auto; padding: 20px; }
.btn.back { background: #ccc; }
</style>
