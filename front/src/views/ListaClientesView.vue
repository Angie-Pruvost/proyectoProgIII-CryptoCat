<template>
  <div class="container">
    <div class="card">
      <div class="row" style="justify-content:space-between; align-items:center;">
        <h1 class="h1">Lista de clientes</h1>
        <input class="input" v-model="busqueda" placeholder="Buscar nombre o email" />
      </div>

      <table class="table" style="margin-top:14px;">
        <thead>
          <tr><th>Nombre</th><th>Email</th><th>Acciones</th></tr>
        </thead>
        <tbody>
          <tr v-for="c in filtrados" :key="c.Id">
            <td>{{ c.Name }}</td>
            <td>{{ c.Email }}</td>
            <td>
              <button class="btn btn-primary" @click="irDashboard(c.Id)">Dashboard</button>
              <button class="btn btn-ghost" @click="irEditar(c.Id)">Editar</button>
              <button class="btn btn-danger" @click="eliminar(c.Id)">Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
const router = useRouter()
const clientes = ref([])
const busqueda = ref('')
const ordenarPor = ref('Name')
const reverso = ref(false)

async function cargar(){
  const r = await fetch('https://localhost:7006/api/Clientes')
  clientes.value = await r.json()
}

function ordenar(campo){ if(ordenarPor.value===campo) reverso.value = !reverso.value; ordenarPor.value = campo }

const filtrados = computed(()=>{
  let lista = clientes.value.filter(c => (c.Name + ' ' + (c.Email||'')).toLowerCase().includes(busqueda.value.toLowerCase()))
  lista.sort((a,b)=> a[ordenarPor.value] < b[ordenarPor.value] ? (reverso.value?1:-1) : (reverso.value?-1:1))
  return lista
})

function irDashboard(id){ router.push(`/cliente-dashboard/${id}`) }
function irEditar(id){ router.push(`/cliente-editar/${id}`) }
async function eliminar(id){
  if(!confirm('Eliminar cliente?')) return
  const res = await fetch(`https://localhost:7006/api/Clientes/${id}`, { method:'DELETE' })
  if(!res.ok){ alert('Error al eliminar'); return }
  await cargar()
}

onMounted(()=> cargar())
</script>

<style scoped></style>
