using System.ComponentModel.DataAnnotations;

namespace criptoApiProyecto.DTOs
{
    public class ClientesDTOs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<TransaccionesDTOs>? Transacciones { get; set; }
    }
}
