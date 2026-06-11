using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webhook.Model
{
    public class StatusContrato
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string IdContrato { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = null!; // Pago, Pendente, Erro

        [Required]
        public DateTime DataUltimaAtualizacao { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UltimoValorPago { get; set; }

        public DateTime? DataUltimoPagamento { get; set; }
    }
}