<template>
  <div class="dashboard-container">
    <button class="back-btn" @click="$router.back()">⬅ Volver</button>

    <section class="card">
      <h2>Cliente</h2>
      <p><strong>Nombre:</strong> {{ cliente.Name }}</p>
      <p><strong>Email:</strong> {{ cliente.Email }}</p>
      <p><strong>ID:</strong> {{ cliente.Id }}</p>

      <button class="btn edit" @click="irAEditarCliente">Editar cliente</button>
      <button class="btn delete" @click="eliminarCliente">Eliminar cliente</button>
    </section>

    <section class="card totals">
      <h2>Resumen</h2>

      <p><strong>Total invertido:</strong> {{ formatMoney(totalInvertido) }}</p>
      <p><strong>Total vendido:</strong> {{ formatMoney(totalVendido) }}</p>
      <p><strong>Ganancia/Pérdida:</strong>
        <span :class="gananciaPerdida >= 0 ? 'green' : 'red'">
          {{ formatMoney(gananciaPerdida) }}
        </span>
      </p>
      <p><strong>Valor actual de la cartera:</strong> {{ formatMoney(valorActualCartera) }}</p>
    </section>

    <section class="card">
      <h2>Cartera Actual</h2>

      <table v-if="Object.keys(cartera).length" class="table">
        <thead>
          <tr>
            <th>Crypto</th>
            <th>Cantidad</th>
            <th>Valor actual ARS</th>
            <th>Total</th>
            <th>Vender</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(c, crypto) in cartera" :key="crypto">
            <td>{{ crypto.toUpperCase() }}</td>
            <td>{{ formatCantidad(c.cantidad) }}</td>
            <td>{{ formatMoney(c.precio) }}</td>
            <td>{{ formatMoney(c.total) }}</td>
            <td>
              <button class="btn sell" @click="abrirVenta(crypto, c.cantidad)">
                Vender
              </button>
            </td>
          </tr>
        </tbody>
      </table>

      <p v-else>El cliente no posee criptomonedas actualmente.</p>
    </section>


    <section class="card">
      <h2>Distribución de cartera</h2>
      <canvas ref="chartRef"></canvas>
    </section>



    <div v-if="modalVenta" class="modal-overlay">
      <div class="modal-content">
        <h3>Vender {{ ventaCrypto.toUpperCase() }}</h3>

        <p><strong>Cantidad disponible:</strong> {{ formatCantidad(ventaCantidadMax) }}</p>

        <label>Cantidad a vender</label>
        <input type="number" step="0.000001" v-model="ventaCantidad" />

        <p class="green">{{ mensajeVenta }}</p>

        <button class="btn sell" @click="confirmarVenta">Confirmar venta</button>
        <button class="btn cancel" @click="modalVenta = false">Cancelar</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import Chart from "chart.js/auto";

const route = useRoute();
const router = useRouter();

const id = route.params.id;


const cliente = ref({});
const transacciones = ref([]);

const totalInvertido = ref(0);
const totalVendido = ref(0);
const gananciaPerdida = ref(0);
const valorActualCartera = ref(0);

const cartera = ref({});

const chartRef = ref(null);
let chart = null;


const modalEditarCliente = ref(false);
const formCliente = ref({ Name: "", Email: "" });

const modalVenta = ref(false);
const ventaCrypto = ref("");
const ventaCantidad = ref(0);
const ventaCantidadMax = ref(0);
const mensajeVenta = ref("");


onMounted(async () => {
  await cargarCliente();
  await cargarTransacciones();
  await calcularCartera();
  dibujarGrafico();
});


async function cargarCliente() {
  const res = await fetch(`https://localhost:7006/api/Clientes/${id}`);
  cliente.value = await res.json();

  formCliente.value.Name = cliente.value.Name;
  formCliente.value.Email = cliente.value.Email;
}


async function cargarTransacciones() {
  const res = await fetch(
    `https://localhost:7006/api/Transacciones/cliente/${id}`
  );
  transacciones.value = await res.json();
}


async function calcularCartera() {
  const resumen = {};

  totalInvertido.value = 0;
  totalVendido.value = 0;

  for (const tx of transacciones.value) {
    if (!resumen[tx.CryptoCode]) {
      resumen[tx.CryptoCode] = { cantidad: 0 };
    }

    if (tx.Action === "purchase") {
      resumen[tx.CryptoCode].cantidad += tx.CryptoAmount;
      totalInvertido.value += tx.Money;
    } else {
      resumen[tx.CryptoCode].cantidad -= tx.CryptoAmount;
      totalVendido.value += tx.Money;
    }
  }


  for (const crypto of Object.keys(resumen)) {
    const precio = await obtenerPrecio(crypto);
    resumen[crypto].precio = precio;
    resumen[crypto].total = resumen[crypto].cantidad * precio;
  }

  cartera.value = resumen;

  valorActualCartera.value = Object.values(resumen)
    .reduce((acc, c) => acc + c.total, 0);

  gananciaPerdida.value = totalVendido.value + valorActualCartera.value - totalInvertido.value;
}


async function obtenerPrecio(crypto) {
  const res = await fetch(`https://criptoya.com/api/bybit/${crypto}/ARS/0.1`);
  const data = await res.json();
  return data.totalAsk;
}


function dibujarGrafico() {
  if (!chartRef.value) return;

  if (chart) chart.destroy();

  chart = new Chart(chartRef.value, {
    type: "pie",
    data: {
      labels: Object.keys(cartera.value),
      datasets: [
        {
          data: Object.values(cartera.value).map((c) => c.total),
        },
      ],
    },
  });
}


function irAEditarCliente() {
  router.push(`/cliente-editar/${id}`);
}

async function guardarCliente() {
  await fetch(`https://localhost:7006/api/Clientes/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(formCliente.value),
  });

  modalEditarCliente.value = false;
  await cargarCliente();
  alert("Cliente actualizado");
}


function abrirVenta(crypto, maxCant) {
  ventaCrypto.value = crypto;
  ventaCantidadMax.value = maxCant;
  ventaCantidad.value = 0;
  mensajeVenta.value = "";
  modalVenta.value = true;
}

async function confirmarVenta() {
  if (ventaCantidad.value <= 0) {
    mensajeVenta.value = "La cantidad debe ser mayor a cero.";
    return;
  }
  if (ventaCantidad.value > ventaCantidadMax.value) {
    mensajeVenta.value = "No podés vender más de lo disponible.";
    return;
  }

  const precio = await obtenerPrecio(ventaCrypto.value);
  const total = ventaCantidad.value * precio;

  const payload = {
    ClienteId: id,
    CryptoCode: ventaCrypto.value,
    Action: "sale",
    CryptoAmount: ventaCantidad.value,
    Money: total,
    Date: new Date().toISOString(),
  };

  await fetch(`https://localhost:7006/api/Transacciones`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });

  modalVenta.value = false;

  await cargarTransacciones();
  await calcularCartera();
  dibujarGrafico();

  alert("Venta realizada");
}


async function eliminarCliente() {
  if (!confirm("¿Seguro que deseas eliminar este cliente?")) return;

  await fetch(`https://localhost:7006/api/Clientes/${id}`, {
    method: "DELETE",
  });

  alert("Cliente eliminado");
  router.push("/");
}


const formatFecha = (f) => new Date(f).toLocaleString("es-AR");
const formatCantidad = (v) => parseFloat(v).toFixed(6);
const formatMoney = (v) => "$" + parseFloat(v).toFixed(2);
</script>

<style scoped>
.dashboard-container {
  max-width: 950px;
  margin: auto;
  padding: 20px;
}


.card.dashboard {
  padding: 20px;
  border-radius: 12px;
  margin-bottom: 20px;
  background: var(--color-card);
  border: 1px solid var(--color-border);
  box-shadow: 0 2px 8px #0002;
}


.table {
  width: 100%;
  border-collapse: collapse;
}

.table th,
.table td {
  border-bottom: 1px solid var(--color-border);
  padding: 10px;
  font-size: 0.95rem;
  color: var(--color-text);
}


.btn {
  padding: 8px 14px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
}

.btn.edit { background: #2764a4; color: #fff; }
.btn.sell { background: #017949; color: #fff; }
.btn.delete { background: #a42f2f; color: #fff; }
.btn.save { background: #237cdb; color: #fff; }
.btn.cancel { background: #814c4c; color: #fff; }

.btn.sell:hover,
.btn.edit:hover,
.btn.delete:hover,
.btn.save:hover,
.btn.cancel:hover {
  filter: brightness(1.15);
}


.green { color: green; }
.red { color: red; }


.back-btn {
  margin-bottom: 12px;
  background: #dedede;
  padding: 8px 12px;
  border-radius: 6px;
  cursor: pointer;
}


.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  backdrop-filter: blur(4px);
  background: #0005;
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  background: #fff;
  padding: 25px;
  width: 350px;
  border-radius: 12px;
  border: 1px solid var(--color-border);
}


.metrica {
  background: linear-gradient(135deg, var(--color-card), #20222480);
  color: var(--color-text);
  border-left: 6px solid var(--color-btn);
  box-shadow: 0 2px 10px #0003;
  padding: 20px;
  border-radius: 12px;
  text-align: center;
}

.metrica h3 {
  font-size: 1.1rem;
  margin-bottom: 8px;
}

.metrica p {
  font-size: 1.4rem;
  font-weight: bold;
}

.tabla-portfolio tr:hover {
  background: rgba(0, 0, 0, 0.05);
  transition: 0.2s;
}

</style>
