<template>
  <div class="container">
    <button @click="$router.back()" class="btn back">⬅ Volver</button>

    <h1>Editar Cliente</h1>

    <div class="card">
      <label>Nombre</label>
      <input v-model="form.Name" />

      <label>Email</label>
      <input v-model="form.Email" />

      <button class="btn save" @click="guardar">Guardar cambios</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";

const route = useRoute();
const router = useRouter();

const id = route.params.id;

const form = ref({
  Name: "",
  Email: ""
});

onMounted(async () => {
  const res = await fetch(`https://localhost:7006/api/Clientes/${id}`);
  const data = await res.json();

  form.value.Name = data.Name;
  form.value.Email = data.Email;
});

async function guardar() {
  await fetch(`https://localhost:7006/api/Clientes/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(form.value)
  });

  alert("Cliente actualizado correctamente");
  router.push(`/cliente-dashboard/${id}`);
}
</script>

<style scoped>
.container {
  max-width: 600px;
  margin: auto;
  padding: 20px;
}

.card {
  background: #fff;
  padding: 20px;
  margin-top: 15px;
  border-radius: 10px;
  box-shadow: 0 2px 8px #0002;
}

.btn {
  padding: 8px 14px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  margin-top: 10px;
}

.btn.save {
  background: #0077ff;
  color: white;
  width: 100%;
}

.btn.back {
  background: #ccc;
}
</style>
