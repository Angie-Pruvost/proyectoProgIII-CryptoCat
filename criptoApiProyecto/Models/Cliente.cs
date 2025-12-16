using System.ComponentModel.DataAnnotations;

namespace criptoApiProyecto.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)] 
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public List<Transaccion> Transacciones { get; set; } = new List<Transaccion>();//previene errores si se quiere acceder a cliente.transacciones 
    }
}
