using Microsoft.AspNetCore.Mvc;
using Webhook.Data;
using Webhook.DTO;
using Webhook.Interface;

namespace Webhook.Service
{
    public class WebhookService : IWebhookService
    {
        private readonly AppDbContext _context;

        public WebhookService(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult ReceberPagamentoWebhook(PagamentoWebhookDto dto)
        {
            return null;
        }

    }
}
