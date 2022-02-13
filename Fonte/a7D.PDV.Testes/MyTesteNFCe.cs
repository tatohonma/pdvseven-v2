using a7D.PDV.Fiscal.NFCe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFe.Classes;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Observacoes;
using NFe.Classes.Informacoes.Pagamento;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Transporte;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Utils;
using NFe.Utils.NFe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace a7D.PDV.Testes
{
    public class MyTesteNFCe
    {
        public nfeProc ObterNfeValidadaEnvia(NFe.Classes.NFe nfe)
        {
            File.WriteAllText(@"..\..\Venda-NFCe-A.xml", nfe.ObterXmlString());

            //Console.WriteLine("ObterUrlConsulta: " + nfe.infNFeSupl.ObterUrlConsulta(nfe, VersaoQrCode.QrCodeVersao2));
            //Console.WriteLine("ObterUrlQrCode: " + nfe.infNFeSupl.ObterUrlQrCode(nfe, VersaoQrCode.QrCodeVersao2, facede.CIdToken, facede.Csc));

            var retorno = NFeFacade.Enviar(nfe);

            var xMotivo = retorno.Retorno.xMotivo;
            Console.WriteLine(xMotivo);

            var nProt = retorno.Retorno.protNFe.infProt.nProt;
            xMotivo = retorno.Retorno.protNFe.infProt.xMotivo;
            if (nProt == null)
            {
                Console.WriteLine("EnviarNFeAssinada() xMotivo: " + xMotivo);
                Assert.Fail(xMotivo);
            }

            var proc = new nfeProc
            {
                NFe = nfe,
                protNFe = retorno.Retorno.protNFe,
                versao = nfe.infNFe.versao
            };

            Console.WriteLine("nProt: " + nProt);

            var xml = proc.ObterXmlString();
            File.WriteAllText(@"..\..\Venda-NFCe-B.xml", xml);

            //var resutX = NFeFacade.ConsultarReciboDeEnvio(proc.protNFe.infProt.chNFe);

            //var proc = facede.ConsultaNFeProtocolo(nfe.infNFe.Id.Substring(3));
            //proc.NFe = nfe;
            //File.WriteAllText(@"..\..\Venda-NFCe-C.xml", proc.ObterXmlString());

            NFeFacade.Imprimir(xml, @"..\..\Venda-Cupom.png");

            return proc;
        }

        public string LerNfeValidadaEnvia(string file, int lote)
        {
            var servicoNFe = new ServicosNFe(ConfiguracaoServico.Instancia);

            var nfe = new NFe.Classes.NFe();
            nfe = nfe.CarregarDeArquivoXml(file);

            var retornoEnvio = servicoNFe.NFeAutorizacao(Convert.ToInt32(lote), IndicadorSincronizacao.Sincrono, new List<NFe.Classes.NFe> { nfe }, false);

            return retornoEnvio.RetornoStr;
        }

        public NFe.Classes.NFe ObterNfeValidada(int numero)
        {

            var nfe = new NFe.Classes.NFe
            {
                infNFe = GetInf(numero)
            };

            nfe = NFeFacade.Assinar(nfe);
            nfe = NFeFacade.Validar(nfe);

            return nfe;
        }

        protected infNFe GetInf(int numero)
        {
            var infNFe = new infNFe
            {
                versao = ConfiguracaoServico.Instancia.VersaoLayout.VersaoServicoParaString(),
                ide = NFeFacade.GetIdentificacao(numero),
                emit = NFeFacade.GetEmitente(),
                dest = GetDestinatario(),
                transp = new transp
                {
                    modFrete = ModalidadeFrete.mfSemFrete //NFCe: Não pode ter frete
                }
            };

            for (var i = 0; i < 2; i++)
            {
                var prod = GetDetalhe(i, infNFe.emit.CRT);
                infNFe.det.Add(prod);
            }

            infNFe.total = GetTotal(infNFe.det);

            infNFe.pag = GetPagamento(infNFe.total.ICMSTot); //NFCe Somente  

            if (infNFe.ide.mod == DFe.Classes.Flags.ModeloDocumento.NFCe & ConfiguracaoServico.Instancia.VersaoLayout != VersaoServico.ve400)
                infNFe.infAdic = new infAdic() { infCpl = "Troco: 10,00" }; //Susgestão para impressão do troco em NFCe


            return infNFe;
        }

        protected virtual List<pag> GetPagamento(ICMSTot icmsTot)
        {
            var valorPagto = (icmsTot.vNF / 2).Arredondar(2);
            VersaoServico versao = ConfiguracaoServico.Instancia.VersaoLayout;
            if (versao != VersaoServico.ve400) // difernte de versão 4 retorna isso
            {
                var p = new List<pag>
                {
                    new pag {tPag = FormaPagamento.fpDinheiro, vPag = valorPagto},
                    new pag {tPag = FormaPagamento.fpCheque, vPag = icmsTot.vNF - valorPagto}
                };
                return p;
            }


            // igual a versão 4 retorna isso
            var p4 = new List<pag>
            {
                //new pag {detPag = new detPag {tPag = FormaPagamento.fpDinheiro, vPag = valorPagto}},
                //new pag {detPag = new detPag {tPag = FormaPagamento.fpCheque, vPag = icmsTot.vNF - valorPagto}}
                new pag
                {
                    detPag = new List<detPag>
                    {
                        new detPag {tPag = FormaPagamento.fpCreditoLoja, vPag = valorPagto},
                        new detPag {tPag = FormaPagamento.fpCreditoLoja, vPag = icmsTot.vNF - valorPagto}
                    }
                }
            };


            return p4;
        }

        protected virtual total GetTotal(List<det> produtos)
        {
            VersaoServico versao = ConfiguracaoServico.Instancia.VersaoLayout;
            var icmsTot = new ICMSTot
            {
                vProd = produtos.Sum(p => p.prod.vProd),
                vDesc = produtos.Sum(p => p.prod.vDesc ?? 0),
                vTotTrib = produtos.Sum(p => p.imposto.vTotTrib ?? 0),
            };

            if (versao == VersaoServico.ve310 || versao == VersaoServico.ve400)
                icmsTot.vICMSDeson = 0;

            if (versao == VersaoServico.ve400)
            {
                icmsTot.vFCPUFDest = 0;
                icmsTot.vICMSUFDest = 0;
                icmsTot.vICMSUFRemet = 0;
                icmsTot.vFCP = 0;
                icmsTot.vFCPST = 0;
                icmsTot.vFCPSTRet = 0;
                icmsTot.vIPIDevol = 0;
            }

            foreach (var produto in produtos)
            {
                if (produto.imposto.IPI != null && produto.imposto.IPI.TipoIPI.GetType() == typeof(IPITrib))
                    icmsTot.vIPI = icmsTot.vIPI + ((IPITrib)produto.imposto.IPI.TipoIPI).vIPI ?? 0;

                if (produto.imposto.ICMS.TipoICMS.GetType() == typeof(ICMS00))
                {
                    icmsTot.vBC = icmsTot.vBC + ((ICMS00)produto.imposto.ICMS.TipoICMS).vBC;
                    icmsTot.vICMS = icmsTot.vICMS + ((ICMS00)produto.imposto.ICMS.TipoICMS).vICMS;
                }
                if (produto.imposto.ICMS.TipoICMS.GetType() == typeof(ICMS20))
                {
                    icmsTot.vBC = icmsTot.vBC + ((ICMS20)produto.imposto.ICMS.TipoICMS).vBC;
                    icmsTot.vICMS = icmsTot.vICMS + ((ICMS20)produto.imposto.ICMS.TipoICMS).vICMS;
                }
                //Outros Ifs aqui, caso vá usar as classes ICMS00, ICMS10 para totalizar
            }

            //** Regra de validação W16-10 que rege sobre o Total da NF **//
            icmsTot.vNF =
                icmsTot.vProd
                - icmsTot.vDesc
                - icmsTot.vICMSDeson.GetValueOrDefault()
                + icmsTot.vST
                + icmsTot.vFCPST.GetValueOrDefault()
                + icmsTot.vFrete
                + icmsTot.vSeg
                + icmsTot.vOutro
                + icmsTot.vII
                + icmsTot.vIPI
                + icmsTot.vIPIDevol.GetValueOrDefault();

            var t = new total { ICMSTot = icmsTot };
            return t;
        }

        protected dest GetDestinatario()
        {
            var dest = new dest(ConfiguracaoServico.Instancia.VersaoLayout)
            {
                //CNPJ = "99999999000191",
                CPF = "19221149870",
            };

            dest.indIEDest = indIEDest.NaoContribuinte; //NFCe: Tem que ser não contribuinte V3.00 Somente
            dest.email = "teste@gmail.com"; //V3.00 Somente
            return dest;
        }

        protected virtual ICMSBasico InformarCSOSN(Csosnicms CST)
        {
            switch (CST)
            {
                case Csosnicms.Csosn101:
                    return new ICMSSN101
                    {
                        CSOSN = Csosnicms.Csosn101,
                        orig = OrigemMercadoria.OmNacional
                    };
                case Csosnicms.Csosn102:
                    return new ICMSSN102
                    {
                        CSOSN = Csosnicms.Csosn102,
                        orig = OrigemMercadoria.OmNacional
                    };
                //Outros casos aqui
                default:
                    return new ICMSSN201();
            }
        }

        protected ICMSBasico InformarICMS(Csticms CST, VersaoServico versao)
        {
            var icms20 = new ICMS20
            {
                orig = OrigemMercadoria.OmNacional,
                CST = Csticms.Cst20,
                modBC = DeterminacaoBaseIcms.DbiValorOperacao,
                vBC = 1.1m,
                pICMS = 18,
                vICMS = 0.20m,
                motDesICMS = MotivoDesoneracaoIcms.MdiTaxi
            };
            if (versao == VersaoServico.ve310)
                icms20.vICMSDeson = 0.10m; //V3.00 ou maior Somente

            switch (CST)
            {
                case Csticms.Cst00:
                    return new ICMS00
                    {
                        CST = Csticms.Cst00,
                        modBC = DeterminacaoBaseIcms.DbiValorOperacao,
                        orig = OrigemMercadoria.OmNacional,
                        pICMS = 18,
                        vBC = 1.1m,
                        vICMS = 0.20m
                    };
                case Csticms.Cst20:
                    return icms20;
                    //Outros casos aqui
            }

            return new ICMS10();
        }

        protected det GetDetalhe(int i, CRT crt)
        {
            var det = new det
            {
                nItem = i + 1,
                prod = GetProduto(i + 1),
                imposto = new imposto
                {
                    vTotTrib = 0.17m,

                    ICMS = new ICMS
                    {
                        //Se você já tem os dados de toda a tributação persistida no banco em uma única tabela, utilize a linha comentada abaixo para preencher as tags do ICMS
                        //TipoICMS = ObterIcmsBasico(crt),

                        //Caso você resolva utilizar método ObterIcmsBasico(), comente esta proxima linha
                        TipoICMS =
                            crt == CRT.SimplesNacional
                                ? InformarCSOSN(Csosnicms.Csosn102)
                                : InformarICMS(Csticms.Cst00, VersaoServico.ve310)
                    },

                    //ICMSUFDest = new ICMSUFDest()
                    //{
                    //    pFCPUFDest = 0,
                    //    pICMSInter = 12,
                    //    pICMSInterPart = 0,
                    //    pICMSUFDest = 0,
                    //    vBCUFDest = 0,
                    //    vFCPUFDest = 0,
                    //    vICMSUFDest = 0,
                    //    vICMSUFRemet = 0
                    //},

                    COFINS = new COFINS
                    {
                        //Se você já tem os dados de toda a tributação persistida no banco em uma única tabela, utilize a linha comentada abaixo para preencher as tags do COFINS
                        //TipoCOFINS = ObterCofinsBasico(),

                        //Caso você resolva utilizar método ObterCofinsBasico(), comente esta proxima linha
                        TipoCOFINS = new COFINSOutr { CST = CSTCOFINS.cofins99, pCOFINS = 0, vBC = 0, vCOFINS = 0 }
                    },

                    PIS = new PIS
                    {
                        //Se você já tem os dados de toda a tributação persistida no banco em uma única tabela, utilize a linha comentada abaixo para preencher as tags do PIS
                        //TipoPIS = ObterPisBasico(),

                        //Caso você resolva utilizar método ObterPisBasico(), comente esta proxima linha
                        TipoPIS = new PISOutr { CST = CSTPIS.pis99, pPIS = 0, vBC = 0, vPIS = 0 }
                    }
                }
            };

            //det.impostoDevol = new impostoDevol() { IPI = new IPIDevolvido() { vIPIDevol = 10 }, pDevol = 100 };

            return det;
        }

        protected virtual prod GetProduto(int i)
        {
            // https://cosmos.bluesoft.com.br/produtos/7894900011517-refrigerante-2l-coca-cola
            return new prod
            {
                cProd = i.ToString().PadLeft(5, '0'),
                cEAN = "7894900011517",
                cEANTrib = "7894900011517",
                xProd = i == 1 && ConfiguracaoServico.Instancia.tpAmb == DFe.Classes.Flags.TipoAmbiente.Homologacao ? "NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL" : "COCA COLA 2L" + i,
                NCM = "22021000",
                CFOP = 5102,
                uCom = "UNID",
                qCom = 1,
                vUnCom = 1.1m,
                vProd = 1.1m,
                vDesc = 0.10m,
                uTrib = "UNID",
                qTrib = 1,
                vUnTrib = 1.1m,
                indTot = IndicadorTotal.ValorDoItemCompoeTotalNF,
                //NVE = {"AA0001", "AB0002", "AC0002"},
                //CEST = ?
            };
        }
    }
}
