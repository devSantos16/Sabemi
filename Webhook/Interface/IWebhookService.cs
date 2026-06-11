using Microsoft.AspNetCore.Mvc;
using Webhook.DTO;

namespace Webhook.Interface
{
    public interface IWebhookService
    {
        public Task ReceberPagamentoWebhook(PagamentoWebhookDto dto);
    }
}
