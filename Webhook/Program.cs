
using Microsoft.EntityFrameworkCore;
using Webhook.Data;
using Webhook.Interface;
using Webhook.Service;

namespace Webhook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("WebhookDb"));
            builder.Services.AddScoped<IWebhookService, WebhookService>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
