using a7D.PDV.Balanca.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Balanca
{
    public static class BalancaServices
    {
        public static bool TemBalanca => tipoBalanca !=  ETipoBalanca.NENHUM;

        private static ETipoBalanca tipoBalanca;
        private static string porta;

        public static void InicializarBalanca(string protocolo, string portaCOM)
        {
            try
            {
                tipoBalanca = (ETipoBalanca)Enum.Parse(typeof(ETipoBalanca), protocolo);
                porta = portaCOM;
            }
            catch (Exception)
            {
            }
        }

        private static IBalanca NovaBalanca() => BalancaFactory.ObterBalanca(tipoBalanca, porta);

        public static async Task<decimal> LerBalancaLoop()
        {
            while(true)
            {
                try
                {
                    var valor = await LerBalanca();
                    if (valor > 0)
                        return valor;
                    else
                        throw new Exception("Balança Vazia");
                }
                catch (Exception ex)
                {
                    var result = MessageBox.Show($"{ex.Message}\nPor favor verifique a balança.\nTentar Novamente?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (result != DialogResult.Yes)
                        break;
                }
            }
            return 0;
        }

        private static async Task<decimal> LerBalanca()
        {
            var result = await NovaBalanca().LerPesoAsync();
            var tentativas = 0;
            while ((result.Status == Status.INSTAVEL || result.Status == Status.SOBREPESO) && tentativas <= 3)
            {
                await Task.Delay(1000);
                result = await NovaBalanca().LerPesoAsync();
                tentativas++;
            }
            if (result.Status == Status.OK)
                return result.Peso;
            else
                throw new Exception($"Balança retornou: {result.Status}: {result.Conteudo}");
        }
    }
}