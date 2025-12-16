<template>
  <div class="container">
    <h1>Resumen General del Sistema</h1>

    <section class="metricas">
      <div class="card" v-for="m in metricas" :key="m.titulo">
        <h2>{{ m.titulo }}</h2>
        <p>{{ m.valor }}</p>
      </div>
    </section>

    <h2>Clientes con mayor inversión</h2>
    <table class="tabla">
      <thead>
        <tr>
          <th>Cliente</th>
          <th>Total Invertido</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="c in ranking" :key="c.id">
          <td>{{ c.name }}</td>
          <td>${{ c.total.toFixed(2) }}</td>
        </tr>
      </tbody>
    </table>

  </div>
</template>

<script setup>
import { ref, onMounted } from "vue"

const metricas = ref([])
const ranking = ref([])

async function cargar() {
  const res = await fetch("https://localhost:7006/api/Clientes")
  const clientes = await res.json()

  // Métricas simples
  metricas.value = [
    { titulo: "Total de clientes", valor: clientes.length },
    { titulo: "Promedio de inversión", valor: 0 },
    { titulo: "Inversiones totales", valor: 0 }
  ]

  // Ranking de inversión
  let lista = []
  for (let c of clientes) {
    const resT = await fetch(`https://localhost:7006/api/Transacciones/cliente/${c.id}`)
    const txs = await resT.json()
    const total = txs
      .filter(t => t.action === "purchase")
      .reduce((acc, t) => acc + t.money, 0)

    lista.push({ id: c.id, name: c.name, total })
  }

  ranking.value = lista.sort((a, b) => b.total - a.total)
}

onMounted(() => cargar())
</script>

<style scoped>
.container { max-width: 1100px; margin: auto; padding: 20px; }
.metricas { display: flex; gap: 20px; margin-bottom: 20px; }
.card { flex: 1; background: var(--color-card); padding: 20px; border-radius: 12px; box-shadow: 0 2px 10px #0003; }
.tabla { width: 100%; border-collapse: collapse; }
td, th { padding: 12px; border-bottom: 1px solid #ccc; }
</style>
