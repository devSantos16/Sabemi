namespace Webhook.DTO
{
    public class PagamentoWebhookDto
    {
        public string IdTransacao { get; set; }

        public string IdContrato { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataPagamento { get; set; }

        public string Status { get; set; }
    }
}
