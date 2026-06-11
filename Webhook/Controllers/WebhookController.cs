using Microsoft.AspNetCore.Mvc;
using Webhook.Data;
using Webhook.Model;

namespace Webhook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WebhookController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ReceberPagamentoWebhook()
        {
            var log = new LogEventoBruto
            {
                Id = Guid.NewGuid(),
                IdTransacao = Guid.NewGuid().ToString(),
                IdContrato = "CTR001",
                Valor = 100,
                DataPagamento = DateTime.UtcNow,
                Status = "Pago"
            };

            _context.LogEventoBruto.Add(log);
            _context.SaveChanges();

            return Ok(new
            {
                Mensagem = "Registro salvo com sucesso",
                Quantidade = _context.LogEventoBruto.Count()
            });
        }

        [HttpGet]
        public IActionResult Obter()
        {
            return Ok(_context.LogEventoBruto.ToList());
        }
    }
}