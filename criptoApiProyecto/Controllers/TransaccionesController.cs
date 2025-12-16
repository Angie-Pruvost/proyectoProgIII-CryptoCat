using criptoApiProyecto.DTOs;
using criptoApiProyecto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace criptoApiProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public TransaccionesController(AppDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        //get todas las transacciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaccion>>> GetTransaccion()
        {
            var transacciones = await _context.Transacciones.Include(t => t.Cliente).ToListAsync();

            var transaccionesDto = transacciones.Select(t => new TransaccionesDTOs
            {
                Id = t.Id,
                ClienteId = t.ClienteId,
                CryptoCode = t.CryptoCode,
                Action = t.Action,
                CryptoAmount = t.CryptoAmount,
                Money = t.Money,
                Date = t.Date
            }).ToList();

            return Ok(transaccionesDto);
        }

        [HttpGet("cliente/{clienteId}")]//get para obtener por cliente/idCliente
        public async Task<ActionResult<IEnumerable<TransaccionesDTOs>>> GetTransaccionesPorCliente(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
                return NotFound("Cliente no encontrado");

            var transacciones = await _context.Transacciones
                .Where(t => t.ClienteId == clienteId)
                .Select(t => new TransaccionesDTOs
                {
                    Id = t.Id,
                    ClienteId = t.ClienteId,
                    CryptoCode = t.CryptoCode,
                    Action = t.Action,
                    CryptoAmount = t.CryptoAmount,
                    Money = t.Money,
                    Date = t.Date
                })
                .ToListAsync();

            return Ok(transacciones);
        }

        [HttpGet("{id}")]//get para obtener una transaccion puntual
        public async Task<IActionResult>GetTransaccion(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);

            if(transaccion == null)
            {
                return NotFound();
            }

            var transaccionDto = new TransaccionesDTOs
            {
                Id = transaccion.Id,
                ClienteId = transaccion.ClienteId,
                CryptoCode = transaccion.CryptoCode,
                Action = transaccion.Action,
                CryptoAmount = transaccion.CryptoAmount,
                Money = transaccion.Money,
                Date = transaccion.Date
            };

            return Ok(transaccionDto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTransaccion([FromBody] TransaccionInputDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Buscar el cliente
            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null)
                return NotFound("Cliente no encontrado");

            // Si es venta, validar saldo
            if (dto.Action == "sale")
            {
                var historial = await _context.Transacciones
                    .Where(t => t.ClienteId == dto.ClienteId && t.CryptoCode == dto.CryptoCode)
                    .ToListAsync();

                var saldo = historial
                    .Where(t => t.Action == "purchase")
                    .Sum(t => t.CryptoAmount)
                  - historial
                    .Where(t => t.Action == "sale")
                    .Sum(t => t.CryptoAmount);

                if (dto.CryptoAmount > saldo)
                    return BadRequest("No tiene suficiente saldo para vender esa cantidad.");
            }

            var transaccion = new Transaccion
            {
                ClienteId = dto.ClienteId,
                CryptoCode = dto.CryptoCode,
                Action = dto.Action,
                CryptoAmount = dto.CryptoAmount,
                Money = dto.Money,
                Date = dto.Date
            };

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return Ok("Transacción registrada correctamente.");
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] TransaccionInputDTO dto)
        {
            var t = await _context.Transacciones.FindAsync(id);

            if (t == null)
                return NotFound();
            t.ClienteId = dto.ClienteId;
            t.CryptoCode = dto.CryptoCode;
            t.Action = dto.Action;
            t.CryptoAmount = dto.CryptoAmount;
            t.Money = dto.Money;
            t.Date = dto.Date;

            await _context.SaveChangesAsync();
            return Ok(t);
        }

        [HttpDelete("{id}")]//eliminar una transaccion por id
        public async Task<IActionResult> DeleteTransaccion(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);

            if(transaccion == null)
            {
                return NotFound();
            }

            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();

            return Ok($"Transaccion con Id {id} eliminada con exito."); 
        }


        

    }
}
