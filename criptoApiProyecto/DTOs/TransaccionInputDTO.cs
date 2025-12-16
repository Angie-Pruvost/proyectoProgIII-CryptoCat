using System.ComponentModel.DataAnnotations;

namespace criptoApiProyecto.DTOs
{
    public class TransaccionInputDTO
    {//ESTE DTOS TIENE SOLO LOS CAMPOS QUE MUESTRO EN EL FRONT
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public string CryptoCode { get; set; } = string.Empty;

        [Required]
        [RegularExpression("purchase|sale", ErrorMessage = "La acción debe ser 'purchase' o 'sale'.")]
        public string Action { get; set; } = string.Empty;

      // [Range(0.01, decimal.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public decimal CryptoAmount { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public decimal Money { get; set; }
    }
}
