using Microsoft.EntityFrameworkCore;
using Webhook.Data;
using Webhook.Model;
using Webhook.Queue;

namespace Webhook.Workers
{
    public class PagamentoWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PagamentoWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (PagamentoQueue.Eventos.TryDequeue(out Guid eventoId))
                {
                    using var scope = _scopeFactory.CreateScope();

                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var logEventoBruto = await context.LogEventoBruto
                            .FirstOrDefaultAsync(x => x.Id == eventoId, stoppingToken);

                    if (logEventoBruto == null)
                        continue;

                    try
                    {
                        // Simulação de processamento pesado
                        await Task.Delay(2000, stoppingToken);

                        StatusContrato contrato = await context.StatusContrato
                            .FirstOrDefaultAsync(x => x.IdContrato == logEventoBruto.IdContrato, stoppingToken);

                        if (contrato == null)
                        {
                            contrato = new StatusContrato
                            {
                                IdContrato = logEventoBruto.IdContrato,
                                Status = logEventoBruto.Status,
                                UltimoValorPago = logEventoBruto.Valor,
                                DataUltimoPagamento = logEventoBruto.DataPagamento,
                                DataUltimaAtualizacao = DateTime.UtcNow
                            };

                            context.StatusContrato.Add(contrato);
                        }
                        else
                        {
                            contrato.Status = logEventoBruto.Status;
                            contrato.UltimoValorPago = logEventoBruto.Valor;
                            contrato.DataUltimoPagamento = logEventoBruto.DataPagamento;
                            contrato.DataUltimaAtualizacao = DateTime.UtcNow;
                        }

                        logEventoBruto.Processado = true;

                        await context.SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logEventoBruto.Processado = false;
                        logEventoBruto.Erro = ex.Message;

                        await context.SaveChangesAsync(stoppingToken);
                    }
                }

                await Task.Delay(500, stoppingToken);
            }
        }
    }
}