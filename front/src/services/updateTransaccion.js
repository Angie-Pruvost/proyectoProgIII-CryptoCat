export async function updateTransaccion(id, data) {
  return await axios.put(`https://localhost:7222/api/transacciones/${id}`, data)
}
