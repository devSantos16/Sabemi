using Microsoft.EntityFrameworkCore;
using Webhook.Model;

namespace Webhook.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<LogEventoBruto> LogEventoBruto { get; set; }

        public DbSet<StatusContrato> StatusContrato { get; set; }
    }
}