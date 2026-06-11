using Microsoft.AspNetCore.Mvc;
using Webhook.Data;
using Webhook.DTO;
using Webhook.Interface;
using Webhook.Model;
using Webhook.Service;

namespace Webhook.Controllers
{
    [ApiController]
    [Route("webhooks/pagamento")]
    public class WebhookController : ControllerBase
    {
        private readonly IWebhookService _webhookService;
        private readonly IConfiguration _configuration;

        public WebhookController(IWebhookService webhookService, IConfiguration configuration)
        {
            _webhookService = webhookService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> ReceberPagamentoWebhook([FromHeader(Name = "X-Api-Key")] string apiKey, [FromBody] PagamentoWebhookDto dto)
        {
            if (apiKey != _configuration["WebhookApiKey"])
            {
                return Unauthorized();
            }

            await _webhookService.ReceberPagamentoWebhook(dto);
            return Ok();
        }
    }
}