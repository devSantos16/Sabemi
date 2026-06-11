using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webhook.Data;
using Webhook.DTO;
using Webhook.Interface;
using Webhook.Model;

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

            try
            {
                StatusContrato contrato = await _context.StatusContrato
                    .FirstOrDefaultAsync(x => x.IdContrato == dto.IdContrato);

                if (contrato == null)
                {
                    contrato = new StatusContrato
                    {
                        IdContrato = dto.IdContrato,
                        Status = dto.Status,
                        UltimoValorPago = dto.Valor,
                        DataUltimoPagamento = dto.DataPagamento,
                        DataUltimaAtualizacao = DateTime.UtcNow
                    };

                    _context.StatusContrato.Add(contrato);
                }

                else
                {
                    contrato.Status = dto.Status;
                    contrato.UltimoValorPago = dto.Valor;
                    contrato.DataUltimoPagamento = dto.DataPagamento;
                    contrato.DataUltimaAtualizacao = DateTime.UtcNow;
                }

                logEventoBruto.Processado = true;
            }

            catch (Exception ex)
            {
                logEventoBruto.Processado = false;
                logEventoBruto.Erro = ex.Message;
            }

            finally
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
