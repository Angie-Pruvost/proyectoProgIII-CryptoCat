using criptoApiProyecto.DTOs;
using criptoApiProyecto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace criptoApiProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _context.Clientes
                     .Select(c => new ClientesDTOs
                     {
                         Id = c.Id,
                         Name = c.Name,
                         Email = c.Email
                     })
                     .ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if(cliente  == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cliente);
            }
        }

        [HttpPost]
        public async Task <IActionResult> PostCliente(Cliente cliente)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCliente), new {id =  cliente.Id}, cliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente (int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado");

            // 🔥 Eliminar primero las transacciones del cliente
            var transacciones = _context.Transacciones.Where(t => t.ClienteId == id);
            _context.Transacciones.RemoveRange(transacciones);

            // 🔥 Eliminar el cliente
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok($"Cliente con ID {id} eliminado correctamente.");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarCliente(int id, [FromBody] Cliente clienteEditado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado");

            // 🔥 Actualizamos los datos
            cliente.Name = clienteEditado.Name;
            cliente.Email = clienteEditado.Email;

            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

    }
}
