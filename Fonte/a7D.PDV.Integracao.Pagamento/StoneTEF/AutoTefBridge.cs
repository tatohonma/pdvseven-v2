using System;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Pagamento.StoneTEF
{
    /// <summary>
    /// Bridge de integração: o PinpadTEF usa estes delegates,
    /// e o PdvServices registra as implementações reais no startup.
    /// </summary>
    public static class AutoTefBridge
    {
        static Func<AutoTefClient> _getClient;
        static Func<Task> _ensureActivatedAsync;

        public static void Register(Func<AutoTefClient> getClient, Func<Task> ensureActivatedAsync)
        {
            _getClient = getClient ?? throw new ArgumentNullException(nameof(getClient));
            _ensureActivatedAsync = ensureActivatedAsync ?? throw new ArgumentNullException(nameof(ensureActivatedAsync));
        }

        public static AutoTefClient GetClient()
        {
            if (_getClient == null)
                throw new InvalidOperationException("AutoTefBridge não foi registrado. Chame AutoTefBridge.Register no startup.");
            return _getClient();
        }

        public static Task EnsureActivatedAsync()
        {
            if (_ensureActivatedAsync == null)
                throw new InvalidOperationException("AutoTefBridge não foi registrado. Chame AutoTefBridge.Register no startup.");
            return _ensureActivatedAsync();
        }
    }
}
