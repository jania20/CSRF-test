using System.Collections.Concurrent;

namespace CSRF_API.Services
{
    public class LedgerService
    {
        private readonly ConcurrentQueue<string> _ledger = new();

        public void Add(string entry) => _ledger.Enqueue(entry);

        public IEnumerable<string> GetAll() => _ledger.ToArray();

        public void Reset()
        {
            while (_ledger.TryDequeue(out _)) { }
        }
    }
}
