using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace a7D.PDV.Iaago.Dialogo
{
    public class IaagoFluxo
    {
        private readonly List<IaagoIntencoes> intencoes = new List<IaagoIntencoes>();

        public readonly List<IaagoLuis> Luis = new List<IaagoLuis>();

        public IEnumerable<string> NomeIntencoes
            => intencoes.Select(i => i.intencao);

        public bool TemIntencao(string intencao)
            => intencoes.Any(i => i.intencao.Equals(intencao, StringComparison.CurrentCultureIgnoreCase));

        public string LoadDirectory(string name)
        {
            string erros = string.Empty;

            lock (intencoes)
            {
                intencoes.Clear();
                Luis.Clear();

                var di = new DirectoryInfo(name);
                foreach (var fi in di.GetFiles("*.json"))
                {
                    try
                    {
                        LoadFile(fi.FullName);
                    }
                    catch (Exception ex)
                    {
                        erros += $"Erro ({fi.FullName}): {ex.Message}";
                    }
                }
            }

            return erros;
        }

        public string LoadFile(string name)
        {
            var fi = new FileInfo(name);
            if (!fi.Exists)
            {
                throw new FileNotFoundException("Arquivo de dialogo não existe: " + fi.FullName);
            }

            var json = File.ReadAllText(fi.FullName);
            var novas = JsonConvert.DeserializeObject<IaagoIntencoes[]>(json);
            if (!novas.Any())
            {
                throw new FileLoadException("Nenhum intenção reconhecida");
            }

            foreach (var nova in novas)
            {
                // Cadastro dos 'Luis'
                if (nova.luis != null)
                {
                    foreach (var l in nova.luis)
                    {
                        var pl = Luis.FindIndex(ls => ls.nome == l.nome);
                        if (pl < 0)
                        {
                            if (!string.IsNullOrEmpty(l.nome))
                            {
                                Luis.Add(l);
                            }
                        }
                        else
                        {
                            Luis[pl] = l;
                        }
                    }
                }

                // Cadastro das 'Intenções'
                var p = intencoes.FindIndex(i => i.intencao == nova.intencao);
                if (p < 0)
                {
                    if (!string.IsNullOrEmpty(nova.intencao))
                    {
                        intencoes.Add(nova);
                    }
                }
                else
                {
                    intencoes[p] = nova;
                }
            }

            // Retorna a primeira nova intenção
            return novas[0].intencao;
        }

        public IaagoIntencoes BuscaIntencao(IaagoVars userIaago)
        {
            if (userIaago.Intencao.Contains("@"))
            {
                userIaago.Intencao = Interpretador.FormataMensagem(userIaago.Intencao, key => userIaago.InterpretaValor(key));
            }
            var intencaoAlvo = intencoes.FirstOrDefault(i => i.intencao.Equals(userIaago.Intencao, StringComparison.CurrentCultureIgnoreCase));
            return intencaoAlvo;
        }

        public IaagoIntencoes ExecutaIntencao(IaagoVars userIaago)
        {
            var intencaoAlvo = BuscaIntencao(userIaago);
            if (intencaoAlvo != null && intencaoAlvo.condicao != null && userIaago.RetornoIntencao == null)
            {
                string nomeVar = "retorno_" + intencaoAlvo.condicao;
                if ((string)userIaago[nomeVar] != "OK")
                {
                    userIaago.RetornoIntencao = userIaago.Intencao; //Transformar em pilha
                    userIaago.RetornoAtribuir = nomeVar;
                    userIaago.Intencao = intencaoAlvo.condicao;
                    return ExecutaIntencao(userIaago);
                }
            }

            intencaoAlvo?.ExecutarAtribuicoesER(userIaago);
            return intencaoAlvo;
        }
    }
}
