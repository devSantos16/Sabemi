using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webhook.Data;
using Webhook.DTO;
using Webhook.Interface;
using Webhook.Model;
using Webhook.Queue;

namespace Webhook.Service
{
    public class WebhookService : IWebhookService
    {
        private readonly AppDbContext _context;

        public WebhookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ReceberPagamentoWebhook(PagamentoWebhookDto dto)
        {
            bool existeTransacao = await _context.LogEventoBruto.AnyAsync(x => x.IdTransacao == dto.IdTransacao);

            if (existeTransacao)
            {
                return;
            }

            LogEventoBruto logEventoBruto = new LogEventoBruto
            {
                Id = Guid.NewGuid(),
                IdTransacao = dto.IdTransacao,
                IdContrato = dto.IdContrato,
                Valor = dto.Valor,
                DataPagamento = dto.DataPagamento,
                Status = dto.Status,
                DataRecebimento = DateTime.UtcNow,
                Processado = false
            };

            _context.LogEventoBruto.Add(logEventoBruto);
            await _context.SaveChangesAsync();

            // Mandar para o worker
            PagamentoQueue.Eventos.Enqueue(logEventoBruto.Id);
        }
    }
}
