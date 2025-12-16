import axios from 'axios'

const API_URL = 'https://localhost:7006/api/Clientes';

export const getClientes = async ()=>{
  try{
    const response = await axios.get(API_URL);
    return response.data;
  }
  catch (error) {
    console.error('Error al obtener las transacciones:', error);
    return [];
  }
  }

