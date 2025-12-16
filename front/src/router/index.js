
import { createRouter, createWebHistory } from 'vue-router'
import AltaClienteView from '@/views/AltaClienteComponentView.vue';


const routes= [
{path:'/',
  name:'Cliente',
  component: AltaClienteView,
},
  {path: '/nueva-compra',
  name: 'NuevaCompra',
  component: ()=> import ('@/views/NuevaCompraView.vue'),
},

  {path: '/historial',
  name: 'Historial',
  component: ()=> import('@/views/HistorialView.vue'),
  },
  {
      path: "/cliente-dashboard/:id",
      name: "ClienteDashboard",
      component: () => import("../views/ClienteDashboardComponent.vue"),
      props: true
  },
  {
  path: "/cliente-editar/:id",
  name: "EditarCliente",
  component: () => import("@/views/EditarClienteView.vue"),
  props: true
},
{
  path: "/transaccion/:id",
  name: "VerTransaccion",
  component: () => import("@/views/VerTransaccionView.vue"),
  props: true
},
{
  path: "/transaccion-editar/:id",
  name: "EditarTransaccion",
  component: () => import("@/views/EditarTransaccionView.vue"),
  props: true
},
{
  path: "/clientes",
  name: "Clientes",
  component: () => import("@/views/ListaClientesView.vue")
},
{
  path: "/ranking",
  name: "Ranking",
  component: () => import("@/views/RankingCriptoView.vue")
},
{
  path: "/historico/:crypto",
  name: "HistoricoCrypto",
  component: () => import("@/views/CryptoHistoricoView.vue"),
  props: true
},
{
  path: "/vender/:id/:crypto",
  name: "Vender",
  component: () => import("@/views/VenderView.vue"),
  props: true
},
{
  path: "/resumen",
  name: "ResumenGeneral",
  component: () => import("@/views/ResumenGeneralView.vue")
}


]
const router = createRouter({
  history: createWebHistory(), routes
})

  export default router;

