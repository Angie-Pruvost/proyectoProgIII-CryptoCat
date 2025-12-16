export async function deleteTransaccion(id) {
  return await axios.delete(`https://localhost:7222/api/transacciones/${id}`)
}
