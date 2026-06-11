using System.ComponentModel.DataAnnotations;

namespace Webhook.Model
{
    public class LogEventoBruto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string IdTransacao { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string IdContrato { get; set; } = null!;

        public decimal Valor { get; set; }

        public DateTime DataPagamento { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = null!;

        public DateTime DataRecebimento { get; set; } = DateTime.UtcNow;

        public bool Processado { get; set; }

        [MaxLength(500)]
        public string? Erro { get; set; }
    }
}
