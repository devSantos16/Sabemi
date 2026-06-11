using System.Collections.Concurrent;

namespace Webhook.Queue
{
    public static class PagamentoQueue
    {
        public static ConcurrentQueue<Guid> Eventos = new();
    }
}