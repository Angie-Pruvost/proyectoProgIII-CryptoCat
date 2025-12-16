<template>
  <div class="container">

    <h1>Dónde conviene comprar / vender</h1>


    <div class="selector">
      <label>Cripto:</label>
      <select v-model="crypto" @change="cargarPrecios">
        <option value="btc">BTC</option>
        <option value="eth">ETH</option>
        <option value="usdt">USDT</option>
        <option value="ada">ADA</option>
        <option value="sol">SOL</option>
      </select>

      <button class="btn" @click="cargarPrecios">Refrescar</button>
    </div>


    <table v-if="exchanges.length" class="tabla">
      <thead>
        <tr>
          <th>Exchange</th>
          <th>Compra (ARS)</th>
          <th>Venta (ARS)</th>
          <th>Me conviene</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="ex in exchanges" :key="ex.nombre">
          <td>{{ ex.nombre }}</td>
          <td>${{ ex.ask }}</td>
          <td>${{ ex.bid }}</td>

          <td>
            <span v-if="ex.ask === mejorCompra" class="ok">Comprar</span>
            <span v-if="ex.bid === mejorVenta" class="ok2">Vender</span>
          </td>
        </tr>
      </tbody>
    </table>

  </div>
</template>

<script setup>
import { ref } from "vue"

const crypto = ref("btc")
const exchanges = ref([])

const mejorCompra = ref(0)
const mejorVenta = ref(0)

const listaExchanges = [
  "binancep2p",
  "bybit",
  "buenbit",
  "ripioexchange",
  "lemoncash",
  "bitso"
]

async function cargarPrecios() {
  exchanges.value = []

  const promesas = listaExchanges.map(async (ex) => {
    try {
      const res = await fetch(`https://criptoya.com/api/${ex}/${crypto.value}/ars/1`)
      const data = await res.json()

      return {
        nombre: ex,
        ask: data.totalAsk ?? data.ask ?? 0,
        bid: data.totalBid ?? data.bid ?? 0
      }
    } catch (err) {
      return {
        nombre: ex,
        ask: 0,
        bid: 0
      }
    }
  })

  exchanges.value = await Promise.all(promesas)

  // Mejor comprar = el precio ASK más bajo
  mejorCompra.value = Math.min(...exchanges.value.map(e => e.ask).filter(n => n > 0))

  // Mejor vender = el BID más alto
  mejorVenta.value = Math.max(...exchanges.value.map(e => e.bid).filter(n => n > 0))
}


cargarPrecios()
</script>

<style scoped>
.container {
  max-width: 800px;
  margin: auto;
  padding: 20px;
}

.selector {
  display: flex;
  gap: 12px;
  align-items: center;
  margin-bottom: 20px;
}

.tabla {
  width: 100%;
  border-collapse: collapse;
  background: var(--color-card);
  border-radius: 12px;
  overflow: hidden;
}

th, td {
  padding: 12px;
  border-bottom: 1px solid #ccc;
}

.ok {
  color: rgb(0, 57, 57);
  font-weight: bold;
}

.ok2 {
  color: rgb(0, 3, 89);
  font-weight: bold;
}

.btn {
  background: var(--color-btn);
  border: none;
  padding: 10px 14px;
  border-radius: 8px;
  cursor: pointer;
  color: white;
}

.btn:hover {
  background: var(--color-btn-hover);
}
</style>
