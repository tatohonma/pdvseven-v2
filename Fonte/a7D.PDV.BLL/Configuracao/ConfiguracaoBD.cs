using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace a7D.PDV.BLL
{
    public abstract class ConfiguracaoBD
    {
        /*
        SELECT Chave,
            '[Config("' + Titulo + '", Valor = ' + ISNULL(('"' + Valor + '"'),'null') +
            ISNULL(', ValoresAceitos="'+ValoresAceitos+'"', '')+
            CASE Obrigatorio WHEN 1 THEN ', Obrigatorio = true' else '' END +
            ')]' as CS 
        FROM tbConfiguracaoBD
        WHERE IDTipoPDV IS NULL;
        */

        protected ConfiguracaoBD(int? idTipoPDV, int? idPDV)
        {
            if (idTipoPDV == -1) // Para construtores vazios (apenas para serialização)
                return;

            var type = this.GetType();

            List<tbConfiguracaoBD> configuracoesPDV = null;
            List<tbConfiguracaoBD> configuracoes = null;

            if (idTipoPDV.HasValue && idPDV.HasValue)
                configuracoesPDV = Repositorio.Listar<tbConfiguracaoBD>(c => c.IDTipoPDV == idTipoPDV && c.IDPDV == idPDV);

            if (idTipoPDV.HasValue)
                configuracoes = Repositorio.Listar<tbConfiguracaoBD>(c => c.IDTipoPDV == idTipoPDV);
            else
                configuracoes = Repositorio.Listar<tbConfiguracaoBD>(c => c.IDTipoPDV == null);

            foreach (var pi in type.GetProperties())
            {
                if (!pi.CanWrite)
                    continue;
                else if (pi.PropertyType.Namespace != "System")
                    continue;

                tbConfiguracaoBD valorDB = null;
                string valor = null;

                try
                {
                    if (idTipoPDV.HasValue && idPDV.HasValue)
                    {
                        valorDB = configuracoesPDV.FirstOrDefault(c => c.Chave.Equals(pi.Name, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == idTipoPDV && c.IDPDV == idPDV);
                        if (valorDB == null) // Valor Padrão
                            valorDB = configuracoes.FirstOrDefault(c => c.Chave.Equals(pi.Name, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == idTipoPDV && c.IDPDV == null);
                    }
                    else if (idTipoPDV.HasValue)
                    {
                        valorDB = configuracoes.FirstOrDefault(c => c.Chave.Equals(pi.Name, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == idTipoPDV && c.IDPDV == null);
                    }
                    else
                    {
                        valorDB = configuracoes.FirstOrDefault(c => c.Chave.Equals(pi.Name, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == null);
                    }

                    valor = valorDB?.Valor;

                    if (pi.PropertyType == typeof(string))
                        pi.SetValue(this, valor);
                    else if (pi.PropertyType == typeof(int))
                    {
                        if (string.IsNullOrEmpty(valor))
                            pi.SetValue(this, 0);
                        else
                            pi.SetValue(this, Int32.Parse(valor));
                    }
                    else if (pi.PropertyType == typeof(long))
                    {
                        if (string.IsNullOrEmpty(valor))
                            pi.SetValue(this, 0L);
                        else
                            pi.SetValue(this, long.Parse(valor));
                    }

                    else if (pi.PropertyType == typeof(bool))
                        pi.SetValue(this, valor == "1");
                    else
                        throw new Exception("Sem Conversor");

                }
                catch (Exception ex)
                {
                    ex.Data.Add(pi.Name, valor);
                    ex.Data.Add("Type", pi.PropertyType.Name);
                    throw new ExceptionPDV(CodigoErro.ECCO, ex);
                }
            }
        }

        public static IEnumerable<ConfiguracaoBDInformation> ListarConfiguracoes()
        {
            var lista = CRUD.Listar(new ConfiguracaoBDInformation()).Cast<ConfiguracaoBDInformation>();
            foreach (var conf in lista)
            {
                if (conf.PDV?.IDPDV != null)
                {
                    conf.PDV = PDV.Carregar(conf.PDV.IDPDV.Value);
                }

                if (conf.TipoPDV?.IDTipoPDV != null)
                {
                    conf.TipoPDV = TipoPDV.Carregar((ETipoPDV)conf.TipoPDV.IDTipoPDV);
                }
                yield return conf;
            }
        }

        public static IEnumerable<ConfiguracaoBDInformation> ListarConfiguracoesPorTipoEChave(int idTipoPdv, string chave)
        {
            return ListarConfiguracoes().Where(c => c.TipoPDV?.IDTipoPDV == idTipoPdv && string.Compare(c.Chave, chave, true) == 0);
        }

        public static void Salvar(ConfiguracaoBDInformation config)
        {
            if (config.IDConfiguracaoBD.HasValue)
                CRUD.Alterar(config);
            else
                CRUD.Adicionar(config);
        }

        public static void Excluir(ConfiguracaoBDInformation config)
        {
            if (config?.IDConfiguracaoBD != null)
                CRUD.Excluir(config);
        }

        public static void AlterarConfiguracaoSistema(string chave, string valor)
        {
            var config = BuscarConfiguracao(chave);
            if (config == null) return;
            else if (config.ConfiguracaoSistema())
            {
                config.Valor = valor;
                Salvar(config);
            }
        }

        public static ConfiguracaoBDInformation BuscarConfiguracao(string chave, int? idTipoPDV = null, int? idPDV = null)
        {
            var objFiltro = new ConfiguracaoBDInformation { Chave = chave };
            if (idTipoPDV.HasValue)
                objFiltro.TipoPDV = new TipoPDVInformation { IDTipoPDV = idTipoPDV.Value };
            if (idPDV.HasValue)
                objFiltro.PDV = new PDVInformation { IDPDV = idPDV.Value };
            return CRUD.Listar(objFiltro)?.FirstOrDefault() as ConfiguracaoBDInformation;
        }

        public static string ValorSistema(EConfig config)
            => ConfiguracaoOuPadrao(config.ToString(), null, null)?.Valor;

        public static string ValorOuPadrao(EConfig config, PDVInformation pdv)
            => ConfiguracaoOuPadrao(config.ToString(), pdv?.IDPDV, pdv?.TipoPDV?.IDTipoPDV)?.Valor;

        public static string ValorOuPadrao(EConfig config, ETipoPDV tipo, int idpdv)
            => ConfiguracaoOuPadrao(config.ToString(), idpdv, (int)tipo)?.Valor;

        public static string ValorOuPadrao(EConfig config, ETipoPDV tipo)
            => ConfiguracaoOuPadrao(config.ToString(), null, (int)tipo)?.Valor;

        public static string ValorOuPadrao(EConfig config, int idPDV)
            => ConfiguracaoOuPadrao(config.ToString(), idPDV, null)?.Valor;

        public static void DefinirValorPadrao(EConfig config, string valor)
        {
            string chave = config.ToString();
            var item = Repositorio.Listar<tbConfiguracaoBD>(c => c.Chave.Equals(chave, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == null).First();
            item.Valor = valor;
            Repositorio.Atualizar(item);
        }

        public static void DefinirValorPadraoTipo(EConfig config, ETipoPDV tipo, string valor)
        {
            int nTipo = (int)tipo;
            string chave = config.ToString();
            var item = Repositorio.Listar<tbConfiguracaoBD>(c => c.Chave.Equals(chave, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == nTipo).First();
            item.Valor = valor;
            Repositorio.Atualizar(item);
        }

        public static void DefinirValorPadraoTipoPDV(EConfig config, ETipoPDV tipo, int idPDV, string valor)
        {
            int nTipo = (int)tipo;
            string chave = config.ToString();
            var item = Repositorio.Listar<tbConfiguracaoBD>(c => c.Chave.Equals(chave, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == nTipo && c.IDPDV == idPDV).FirstOrDefault();
            if (item == null)
            {
                item = Repositorio.Listar<tbConfiguracaoBD>(c => c.Chave.Equals(chave, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == nTipo).FirstOrDefault();
                if (item == null)
                {
                    item = new tbConfiguracaoBD
                    {
                        Chave = chave,
                        IDTipoPDV = (int)tipo,
                        Titulo = "",
                        Obrigatorio = false
                    };
                }
                else
                    item.IDConfiguracaoBD = null;

                item.IDPDV = idPDV;
                item.Valor = valor;
                Repositorio.Inserir(item);
            }
            else
            {
                item.Valor = valor;
                Repositorio.Atualizar(item);
            }
        }

        public static tbConfiguracaoBD ConfiguracaoOuPadrao(string chave, int? idPdv = null, int? idTipoPDV = null, bool? sistema = false) //, int idTipoPdvAlternativo = 0)
        {
            tbConfiguracaoBD config = null;
            List<tbConfiguracaoBD> configuracoes;

            if (idTipoPDV.HasValue)
                configuracoes = Repositorio.Listar<tbConfiguracaoBD>(c => c.Chave.Equals(chave, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == idTipoPDV).ToList();
            else if (idPdv.HasValue)
                configuracoes = Repositorio.Listar<tbConfiguracaoBD>(c => c.Chave.Equals(chave, StringComparison.CurrentCultureIgnoreCase)).ToList();
            else
                configuracoes = Repositorio.Listar<tbConfiguracaoBD>(c => c.Chave.Equals(chave, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == null).ToList();

            if (idPdv.HasValue)
            {
                // retorna o valor configurado pelo PDV
                config = configuracoes.Where(c => c.IDPDV == idPdv).FirstOrDefault();
                if (config != null)
                    return config;
            }

            // retorna o padrão
            config = configuracoes.Where(c => c.IDPDV == null).FirstOrDefault();

            if (idTipoPDV.HasValue && config == null && sistema == true)
                config = Repositorio.Carregar<tbConfiguracaoBD>(c => c.Chave.Equals(chave, StringComparison.CurrentCultureIgnoreCase) && c.IDTipoPDV == null);

            return config;

        }
    }
}
