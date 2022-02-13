using a7D.PDV.Integracao.Pagamento;
using System;
using System.IO;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Pagamento.NTKTEF
{
    public class NTKPinpadPayGo : ITEF
    {
        private const string path = @"C:\PAYGO\Resp";
        private const int MaxTimeOut = 60 * 60 * 4; // Sleep de 250ms
        private const int MinTimeOut = 5 * 4;

        private bool aguardar = true;
        private int TimeOut = MaxTimeOut;
        private bool ClientePayGo = false;
        private bool iniciado = false;
        public string Log => null;

        internal NTKValores Valores { get; private set; }
        public Exception Erro { get; private set; }
        public string Bandeira => Valores[NTKCampos.NomeCartaoAdministradora];
        public string Adquirente => Valores[NTKCampos.RedeAdquirente];
        public decimal Valor => int.Parse(Valores[NTKCampos.ValorTotal]) / 100m;
        public string Mensagem { get; private set; }
        public string Autorizacao => Valores[NTKCampos.CodigoAutorizacao];
        public string ViaEstabelecimento
        {
            get
            {
                var via = "";
                var viasComprovante = Valores[NTKCampos.ViasComprovante];
                var TamanhoViaEstabelecimento = Valores[NTKCampos.TamanhoViaEstabelecimento];

                if (viasComprovante == "2" || viasComprovante == "3" || viasComprovante == null)
                {
                    if (TamanhoViaEstabelecimento == "0" || TamanhoViaEstabelecimento == null)
                    {
                        via = Valores[NTKCampos.ViaUnica];
                    }
                    else
                    {
                        via = Valores[NTKCampos.ViaEstabelecimentoComprovante];
                    }
                }

                if (!String.IsNullOrWhiteSpace(via))
                    return via.Replace("\"", "");
                else
                    return "";
            }
        }

        public string ViaCliente
        {
            get
            {
                var via = "";
                var viasComprovante = Valores[NTKCampos.ViasComprovante];
                var tamanhoCupomReduzido = Valores[NTKCampos.TamanhoCupomReduzido];
                var TamanhoViaCliente = Valores[NTKCampos.TamanhoViaCliente];

                if (viasComprovante == "1" || viasComprovante == "3" || viasComprovante == null)
                {
                    if (tamanhoCupomReduzido == "0" || tamanhoCupomReduzido == null)
                    {
                        if (TamanhoViaCliente == "0" || TamanhoViaCliente == null)
                        {
                            via = Valores[NTKCampos.ViaUnica];
                        }
                        else
                        {
                            via = Valores[NTKCampos.ViaClienteComprovante];
                        }
                    }
                    else
                    {
                        via = Valores[NTKCampos.CupomReduzido];
                    }
                }

                if (!String.IsNullOrWhiteSpace(via))
                    return via.Replace("\"", "");
                else
                    return "";
            }
        }

        public bool PrecisaSelecionar => false;

        public bool Debito
        {
            get
            {
                // 10: Cartão de crédito – à vista
                // 11: Cartão de crédito – parcelado pelo Estabelecimento 
                // 12: Cartão de crédito – parcelado pelo Emissor 
                // 20: Cartão de débito – à vista 
                // 22: Cartão de débito – parcelado pelo Estabelecimento 
                // 21: Cartão de débito – pré-datado 
                // 24: Cartão de débito – pré-datado forçada 
                string tipo = Valores[NTKCampos.TipoTransacao];
                return tipo == "20" || tipo == "21" || tipo == "22" || tipo == "24";
            }
        }

        public bool PagamentoConfirmado
        {
            get
            {
                if (Convert.ToInt32(Valores[NTKCampos.TamanhoViaCliente]) > 0 ||
                    Convert.ToInt32(Valores[NTKCampos.TamanhoViaEstabelecimento]) > 0 ||
                    Convert.ToInt32(Valores[NTKCampos.TamanhoViaUnica]) > 0 ||
                    Convert.ToInt32(Valores[NTKCampos.TamanhoCupomReduzido]) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Retorna o resultado de uma requisição identificada
        /// </summary>
        public NTKPinpadPayGo(NTKValores valores, bool start)
        {
            iniciado = start;
            Mensagem = "Iniciando";
            Valores = valores;
        }

        public bool Processando()
        {
            if (!iniciado)
            {
                iniciado = true;
                NTKBuilder.Commit(Valores);
            }

            var di = new DirectoryInfo(path);
            var files = di.GetFiles();
            if (!ClientePayGo && files.Length == 0 && MaxTimeOut - MinTimeOut > TimeOut)
            {
                Mensagem = "O 'Cliente PayGo' não respondeu a tempo";
                return false; // Timeout
            }
            else if (TimeOut-- < 0)
            {
                Mensagem = "Tempos esgotado para realizar o pagamento";
                return false; // Timeout
            }

            foreach (var fi in files)
            {
                if (fi.Name.ToLower().EndsWith(".sts"))
                {
                    ClientePayGo = true; // Libera mais timeout
                    fi.Delete(); //Apaga o status requisição

                    if (Valores.Etapa == NTKEtapa.VendaEnviada)
                    {
                        Mensagem = Valores[NTKCampos.MensagemOperador];
                        Valores.Etapa = NTKEtapa.VendaRecebida;
                    }
                    else if (Valores.Etapa == NTKEtapa.ConfirmacaoEnviada)
                    {
                        Mensagem = Valores[NTKCampos.MensagemOperador];
                        Valores.Etapa = NTKEtapa.ConfirmacaoRecebida;

                        return false;
                    }
                    else if (Valores.Etapa == NTKEtapa.ADM && Valores[NTKCampos.Comando] != NTKComandos.NCN.ToString())
                    {
                        Mensagem = Valores[NTKCampos.MensagemOperador];
                    }
                    else if (Valores.Etapa == NTKEtapa.ConfirmacaoADM)
                    {
                        Mensagem = Valores[NTKCampos.MensagemOperador];

                        return false;
                    }
                    else if (Valores.Etapa == NTKEtapa.ADM && Valores[NTKCampos.Comando] == NTKComandos.NCN.ToString())
                    {
                        Mensagem = Valores[NTKCampos.MensagemOperador];

                        return false;
                    }

                    continue;
                }
                else if (!fi.Name.EndsWith(".001"))
                {
                    continue;
                }

                try
                {
                    var linhas = File.ReadAllLines(fi.FullName);
                    Valores.LeLinhas(linhas, fi.Name);

                    try
                    {
                        string cFile = Path.Combine(@"C:\PayGo\TEF", string.Format(@"{0:yyyyMMdd-HHmmss}-{1}.AUT", DateTime.Now, Valores[NTKCampos.CodigoAutorizacao]));
                        fi.CopyTo(cFile);
                    }
                    catch (Exception)
                    {
                    }

                    fi.Delete(); //Apaga a o retorno da  requisição 

                    if (Valores.Etapa == NTKEtapa.VendaRecebida)
                    {
                        var conf = NTKBuilder.ConfirmaVenda(Valores);
                        var tef2 = conf.IniciaTransacao();
                    }
                    else if (Valores.Etapa == NTKEtapa.ADM)
                    {
                        if (String.IsNullOrEmpty(Valores[NTKCampos.StatusConfirmacao]))
                        {
                            Mensagem = Valores[NTKCampos.MensagemOperador];
                            return false;
                        }
                        else
                        {
                            Valores.Etapa = NTKEtapa.ConfirmacaoADM;

                            var conf = NTKBuilder.ConfirmarAdministracao(Valores);
                            var tef2 = conf.IniciaTransacao();
                        }
                    }
                    else if (Valores.Etapa == NTKEtapa.ConfirmacaoADM)
                    {
                        Mensagem = Valores[NTKCampos.MensagemOperador];
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Erro = ex;
                    return false;
                }
            }

            return true; // Continua processando
        }

        public async Task AguardaTransacao(StatusUpdateCallBack respostaTEF)
        {
            bool emProcesso;
            do
            {
                emProcesso = Processando();
                respostaTEF?.Invoke(this);
                await Task.Delay(250);
            } while (emProcesso && aguardar);
        }

        public void Cancelar()
        {
            Mensagem = "Cancelado";
            aguardar = false;
        }

        public void Estornar()
        {
        }

        public void DefinirMetodoPagamento(MetodoPagamento metodo, int parcelas)
        {
            if (iniciado)
                return;

            Valores[NTKCampos.Operacao] = "1";
            if (metodo == MetodoPagamento.Debito)
            {
                Valores[NTKCampos.TipoCartao] = "2";
                Valores[NTKCampos.TipoFinanciamento] = "1";
                Valores[NTKCampos.TipoTransacao] = "20";
            }
            else
            {
                // Credito a Vista
                Valores[NTKCampos.TipoCartao] = "1";
                Valores[NTKCampos.TipoFinanciamento] = "1";
                Valores[NTKCampos.TipoTransacao] = "10";
                Valores[NTKCampos.Parcelas] = "01";
            }
        }

        public static bool TransacaoPendente()
        {
            var di = new DirectoryInfo(@"C:\PayGo\TEF");
            if (!di.Exists)
                di.Create();

            var dirResp = new DirectoryInfo(path);
            if (dirResp.GetFiles().Length > 0)
            {
                foreach (System.IO.FileInfo file in dirResp.GetFiles())
                {
                    file.Delete();
                }

                return true;
            }
            else
                return false;
        }
    }
}