<template>
  <div class="container">
    <div class="card">
      <h2>Registro de Cliente</h2>

      <form @submit.prevent="handleSubmit">

        <div class="form-group">
          <label for="name">Nombre y Apellido</label>
          <input id="name" v-model="name" type="text" placeholder="Ej: Juan Pérez" />
        </div>

        <div class="form-group">
          <label for="email">Email</label>
          <input id="email" v-model="email" type="email" placeholder="ejemplo@mail.com" />
        </div>

        <button type="submit" class="btn-submit">Guardar Cliente</button>
      </form>

      <p v-if="error" class="error-text">{{ error }}</p>
      <p v-if="success" class="success-text">{{ success }}</p>

    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";

const name = ref("");
const email = ref("");
const error = ref("");
const success = ref("");

const validateEmail = (email) =>
  /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

const cleanCliente = () => ({
  name: name.value.trim(),
  email: email.value.trim(),
  transacciones: []
});

const handleSubmit = async () => {
  error.value = "";
  success.value = "";

  if (!name.value.trim())
    return (error.value = "El nombre no puede estar vacío.");

  if (!email.value.trim())
    return (error.value = "El email no puede estar vacío.");

  if (!validateEmail(email.value))
    return (error.value = "El email no es válido.");

  try {
    const cliente = cleanCliente();

    const response = await fetch("https://localhost:7006/api/Clientes", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(cliente)
    });

    if (!response.ok) {
      const err = await response.json();
      error.value = err.message || "Error al guardar el cliente.";
      return;
    }

    success.value = "Cliente guardado correctamente.";
    name.value = "";
    email.value = "";

  } catch {
    error.value = "No se pudo conectar al servidor.";
  }
};
</script>

<style scoped>

.container {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 1.5rem;
  min-height: calc(100vh - 80px);
}


.card {
  width: 100%;
  max-width: 420px;
  padding: 1.5rem;
}

.card h2 {
  margin-bottom: 20px;
  font-size: 1.4rem;
  color: var(--color-text);
  text-align: center;
  font-weight: 700;
}


.form-group {
  margin-bottom: 18px;
}

label {
  font-size: 0.95rem;
  margin-bottom: 6px;
  color: #444;
  display: block;
}

input {
  width: 100%;
  padding: 10px 12px;
  font-size: 0.95rem;
  border: 1px solid var(--color-border);
  border-radius: 8px;
  outline: none;
}

input:focus {
  border-color: var(--color-btn);
  box-shadow: 0 0 4px rgba(13, 110, 253, 0.3);
}


.btn-submit {
  width: 100%;
  background: var(--color-btn);
  color: white;
  padding: 12px;
  font-size: 1rem;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: 0.25s;
}

.btn-submit:hover {
  background: var(--color-btn-hover);
}


.error-text {
  margin-top: 12px;
  color: var(--color-danger);
  text-align: center;
  font-weight: 500;
}

.success-text {
  margin-top: 12px;
  color: var(--color-success);
  text-align: center;
  font-weight: 500;
}


@media (min-width: 900px) {
  .card {
    max-width: 480px;
    padding: 2rem;
  }
}

</style>
