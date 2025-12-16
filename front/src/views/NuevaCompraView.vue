
<template>
  <div class="compra-wrapped">
    <h2>Nueva Compra de Criptomoneda</h2>
<div class="page">
  <form @submit.prevent="handleSubmit">

      <div class="form-group">
        <label for="crypto">Criptomoneda</label>
        <select id="crypto" v-model="cryptoCode">
          <option disabled value="">Seleccionar</option>
          <option v-for="crypto in cryptos" :key="crypto" :value="crypto">
            {{ crypto.toUpperCase() }}
          </option>
        </select>
        <span v-if="errors.cryptoCode" class="error-text">{{ errors.cryptoCode }}</span>
      </div>


      <div class="form-group">
        <label for="amount">Cantidad</label>
        <input
          id="amount"
          type="number"
          step="0.01"
          min="0.01"
          v-model.number="cryptoAmount"
          placeholder="Ej: 0.05"
          @input="calculateEstimatedPrice"
        />
        <span v-if="errors.cryptoAmount" class="error-text">{{ errors.cryptoAmount }}</span>
        <p>Total en ARS: {{ money.toFixed(2) }}</p>
      </div>


      <div class="form-group">
        <label for="client">Cliente</label>
        <select id="client" v-model="clientId">
          <option value="">Seleccionar</option>
          <option v-for="client in clients" :key="client.Id" :value="client.Id">
            {{ client.Name }}
          </option>
        </select>
        <span v-if="errors.clientId" class="error-text">{{ errors.clientId }}</span>
      </div>


      <div class="form-group">
        <label for="date">Fecha y Hora</label>
        <input type="datetime-local" id="date" v-model="date" />
        <span v-if="errors.date" class="error-text">{{ errors.date }}</span>
      </div>


      <div v-if="estimatedPrice">
        <p v-if="precio">Precio actual (ARS): ${{ money.toFixed(2) }}</p>

      </div>

      <button type="submit">Registrar Compra</button>

      <p v-if="successMessage" class="success-text">{{ successMessage }}</p>
      <p v-if="errors.api" class="error-text">{{ errors.api }}</p>
    </form>
</div>

  </div>
</template>

<script setup>
import { ref, onMounted,watch} from 'vue'
import '../styles/nuevaCompra.css'
const cryptoCode = ref('')
const cryptoAmount = ref('')
const clientId = ref ('')
const date = ref('')
const money = ref(0)
const precio = ref('')
const clients = ref([])
const successMessage = ref('')
const errors = ref({})
const estimatedPrice = ref('')

const cryptos =ref (["BTC", "ETH", "USDT", "BNB"])


onMounted(async () => {
  try {
    const res = await fetch('https://localhost:7006/api/Clientes')
    clients.value = await res.json()
  } catch (err) {
    console.error('Error al cargar clientes:', err)
  }
})

watch([cryptoCode, cryptoAmount],  async () => {
  console.log('Cambiaron cryptoCode o cryptoAmount:', cryptoCode.value, cryptoAmount.value)
  if (cryptoCode.value && cryptoAmount.value) {
    try {
      const res = await fetch(`https://criptoya.com/api/bybit/${cryptoCode.value}/ARS/0.1`);
      const data = await res.json()

      console.log("Respuesta de CryptoYa:", data)//prueba

      const precio = data?.totalAsk

      if (precio && !isNaN(precio)) {
        money.value = Number(cryptoAmount.value) * Number(precio)
      } else {
        console.error("Precio inválido:", precio)
        money.value = 0
      }

    } catch (err) {
      console.error('Error al obtener precio:', err)
      money.value = 0
    }
  } else {
    money.value = 0
  }
})


const validateForm = () => {
  errors.value = {}
  if (!cryptoCode.value) errors.value.cryptoCode = 'Seleccioná una criptomoneda.'
  if (!cryptoAmount.value || cryptoAmount.value <= 0) errors.value.cryptoAmount = 'Cantidad inválida.'
  if (!clientId.value) errors.value.clientId = 'Seleccioná un cliente.'
  if (!date.value) errors.value.date = 'Seleccioná la fecha.'
  return Object.keys(errors.value).length === 0
}

const handleSubmit = async () => {
  console.log("Valores actuales:",cryptoAmount.value, money.value)

  if (!validateForm()) return

  const payload = {
  ClienteId: parseInt(clientId.value),
  CryptoCode: cryptoCode.value,
  Action: 'purchase',
  Money: money.value,
  CryptoAmount: parseFloat(cryptoAmount.value),
  Date: new Date(date.value).toISOString(),

  }

console.log("PAYLOAD ENVIADO AL BACKEND:", JSON.stringify(payload, null, 2))

  try {

    const res = await fetch('https://localhost:7006/api/Transacciones', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    })

    if (!res.ok) {
      const err = await res.text()
      throw new Error(err)
    }

    successMessage.value = 'Compra registrada correctamente.'
    cryptoCode.value = ''
    cryptoAmount.value = ''
    clientId.value = ''
    date.value = ''
    money.value = 0
  } catch (error) {
    console.error('Error al registrar la compra:', error)
    errors.value.api = 'No se pudo registrar la compra.'
  }
}

</script>


<style scoped>

.compra-wrapper {
  width: 100%;
  max-width: 550px;
  margin: 2rem auto;
}


.form-compra {
  background: var(--color-card) !important;
  border-radius: 12px;
  padding: 1.5rem;
  border: 1px solid var(--color-border);
}


.form-compra label {
  color: var(--color-text);
  font-weight: 600;
  margin-bottom: .3rem;
}


.form-compra input,
.form-compra select {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  color: var(--color-text);
  padding: .7rem;
  border-radius: 8px;
  width: 100%;
  font-size: 1rem;
}

.form-compra input:focus,
.form-compra select:focus {
  border-color: var(--color-btn);
  box-shadow: 0 0 4px var(--color-btn);
}


.form-compra button {
  margin-top: 1rem;
  width: 100%;
  background: var(--color-btn) !important;
  color: white;
  border: none;
  padding: 0.75rem;
  border-radius: 10px;
  font-weight: 600;
  cursor: pointer;
  transition: 0.25s;
}

.form-compra button:hover {
  background: var(--color-btn-hover) !important;
}

</style>


