using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.ERPCake.DTO;
using a7D.PDV.Integracao.ERPCake.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace a7D.PDV.Integracao.ERPCake.Sync
{
    public static class GenericSync
    {
        static readonly Dictionary<Type, object> cache = new Dictionary<Type, object>();

        internal static void Insert<Tcake>(Tcake item)
            where Tcake : ModelCake
        {
            if (!cache.TryGetValue(typeof(Tcake), out object listaCache))
                return;

            var cakeItens = (IDictionary<int, Tcake>)listaCache;
            cakeItens.Add(item.id.Value, item);
        }

        internal static Tcake FirstOrDefault<Tcake>(Func<Tcake, bool> find)
            where Tcake : ModelCake
        {
            if (!cache.TryGetValue(typeof(Tcake), out object listaCache))
                return null;

            var cakeItens = (IDictionary<int, Tcake>)listaCache;

            return cakeItens.Values.FirstOrDefault(find);
        }

        internal static Tcake GetByID<Tcake>(string codigoERP)
            where Tcake : ModelCake
        {
            if (int.TryParse(codigoERP, out int id))
                return GetByID<Tcake>(id);
            else
                return null;
        }

        internal static Tcake GetByID<Tcake>(int id)
            where Tcake : ModelCake
        {
            if (!cache.TryGetValue(typeof(Tcake), out object listaCache))
                return null;

            var cakeItens = (IDictionary<int, Tcake>)listaCache;
            if (cakeItens.TryGetValue(id, out Tcake itemERP))
                return itemERP;
            else
                return null;
        }

        internal static bool ContainsID<Tcake>(string codigoERP)
            where Tcake : ModelCake
        {
            if (int.TryParse(codigoERP, out int id))
                return ContainsID<Tcake>(id);
            else
                return false;
        }

        internal static bool ContainsID<Tcake>(int id)
            where Tcake : ModelCake
        {
            if (!cache.TryGetValue(typeof(Tcake), out object listaCache))
                return false;

            var cakeItens = (IDictionary<int, Tcake>)listaCache;
            return cakeItens.ContainsKey(id);
        }

        private static Dictionary<int, Tcake> All<Tcake>(this ICakeBase cake)
            where Tcake : ModelCake
        {
            var cakeItens = new Dictionary<int, Tcake>();
            int offset = 0;
            int limit = 50;
            do
            {
                cake.AddLog($" !!! ERP Lendo: {typeof(Tcake).Name} {offset + 1}-{offset + limit}");
                var partialItens = cake.api.All<Tcake>(offset, limit);

                foreach (var item in partialItens)
                    cakeItens.Add(item.id.Value, item);

                if (partialItens.Length < limit)
                    break;

                offset += limit;
            } while (true);

            return cakeItens;
        }

        public static void CacheClear(this ICakeBase cake) => cache.Clear();

        public static IDictionary<int, Tcake> CacheGetAll<Tcake>(this ICakeBase cake)
            where Tcake : ModelCake
        {
            IDictionary<int, Tcake> erpItens;
            if (cache.TryGetValue(typeof(Tcake), out object listaCache))
                erpItens = (IDictionary<int, Tcake>)listaCache;
            else
            {
                erpItens = cake.All<Tcake>();
                cache.Add(typeof(Tcake), erpItens);
            }
            return erpItens;
        }

        public static void Sincronizar<Tpdv, Tcake>(this ICakeBase cake, bool obrigatorio)
            where Tpdv : class, EF.IERP, EF.IERPSync, new()
            where Tcake : ModelCake
        {
            try
            {
                var pdvItens = EF.Repositorio.Listar<Tpdv>().ToDictionary(row => row.myID());

                var erpItens = cake.CacheGetAll<Tcake>();

                cake.AddLog($" === ERP {typeof(Tcake).Name}: {erpItens.Count} PDV {typeof(Tpdv).Name}: {pdvItens.Count}");

                if (typeof(Tcake) != typeof(Payment_Form)) // Pagamento só sobe!
                    cake.Download(erpItens, pdvItens, obrigatorio);

                cake.Upload(pdvItens, erpItens, obrigatorio);
            }
            catch (Exception ex)
            {
                if (obrigatorio)
                    throw new ExceptionPDV(CodigoErro.EE02, ex);
                else
                    cake.AddLog(ex, true);

                Thread.Sleep(5000);
            }
        }

        private static void Download<Tpdv, Tcake>(this ICakeBase cake, IDictionary<int, Tcake> erpItens, IDictionary<int, Tpdv> pdvItens, bool obrigatorio)
            where Tpdv : class, EF.IERP, EF.IERPSync, new()
            where Tcake : ModelCake
        {
            int n = 0;
            int i = 0;
            int a = 0;
            int e = 0;
            foreach (var itemERP in erpItens.Values)
            {
                try
                {
                    n++;
                    if (!itemERP.RequerAlteracaoPDV(cake.UltimoSync, out int id))
                        continue;

                    bool inserir = false;
                    if (pdvItens.TryGetValue(id, out Tpdv itemPDV))
                        a++;
                    else
                    {
                        inserir = true;
                        i++;
                    }

                    string info = "";
                    if (erpItens.Count > 100) // mostra percentual quando a lista tem mais de 100 itens
                        info = $"{(100 * n / (double)erpItens.Count).ToString("N1")}% ";

                    itemPDV = DTOdownload.UpdateOrCreate(itemERP, itemPDV, out string log);
                    // Durante o download os dados são gravados no PDV apenas

                    if (inserir)
                    {
                        EF.Repositorio.Inserir(itemPDV);
                        itemERP.SetCode(itemPDV.myID().ToString()); // Atualiza o Id do cache!
                    }
                    else
                        EF.Repositorio.Atualizar(itemPDV);

                    cake.AddLog($" <<< ERP {info} {itemERP.id} {log} {itemPDV}");
                }
                catch (Exception ex)
                {
                    ex.Data.Add("itemERP", itemERP.ToString());
                    if (obrigatorio)
                        throw ex;
                    else
                        cake.AddLog(ex);
                }
            }

            if (i > 0 || a > 0 || e > 0)
                cake.AddLog($" <<< SYNC {typeof(Tpdv).Name} QTD: {n} Inseridos: {i} Atualizados: {a} erros: {e}");
        }

        private static void Upload<Tpdv, Tcake>(this ICakeBase cake, IDictionary<int, Tpdv> pdvItens, IDictionary<int, Tcake> erpItens, bool obrigatorio)
            where Tpdv : class, EF.IERP, EF.IERPSync, new()
            where Tcake : ModelCake
        {
            int n = 0;
            int i = 0;
            int a = 0;
            int e = 0;
            foreach (var itemPDV in pdvItens.Values)
            {
                n++;
                string info = "";
                try
                {
                    bool inserir = false;
                    if (int.TryParse(itemPDV.CodigoERP, out int id)
                     && erpItens.TryGetValue(id, out Tcake itemERP))
                    {
                        if (itemPDV.RequerAlteracaoERP(cake.UltimoSync))
                            a++;
                        else
                            continue;
                    }
                    else if (itemPDV.CodigoERP?.Contains(",") == true)
                        continue; // Multipla referencia já inserida!
                    else
                    {
                        inserir = true;
                        itemERP = null;
                        i++;
                    }

                    if (pdvItens.Count > 100) // mostra percentual quando a lista tem mais de 100 itens
                        info = $"{(100 * n / (double)pdvItens.Count).ToString("N1")}% ";

                    string log = string.Empty;

                    if (itemPDV is tbTipoPagamento pay && pay.IDGateway > 0 && pay.IDGateway != (int)EGateway.ContaCliente)
                    {
                        if (!inserir)
                            throw new Exception($"Gateway de pagamento '{pay.Nome}' não permite alteração");

                        var itemCredito = itemERP as Payment_Form;
                        itemCredito = DTOupload.UpdateOrCreate(itemPDV, itemCredito, ref log);
                        itemCredito.name += " (crédito)";
                        itemCredito.payment_type = DTOtradutor.PaymentType((int)EMetodoPagamento.Credito);
                        var result1 = cake.api.Save(itemCredito);
                        cake.api.Save(new Payment_Form_Condition()
                        {
                            payment_form = result1.id.Value,
                            days_parcel = 30,
                            parcel = 1
                        });
                        info += " crédito " + log + " " + itemPDV.CodigoERP;

                        Payment_Form itemDebito = null;
                        itemDebito = DTOupload.UpdateOrCreate(itemPDV, itemDebito, ref log);
                        itemDebito.name += " (débito)";
                        itemDebito.payment_type = DTOtradutor.PaymentType((int)EMetodoPagamento.Debito);
                        var result2 = cake.api.Save(itemDebito);
                        cake.api.Save(new Payment_Form_Condition()
                        {
                            payment_form = result2.id.Value,
                            days_parcel = 2,
                            parcel = 1
                        });
                        info += " débito " + log;

                        pay.CodigoERP = $"{result1.id},{result2.id}";
                        EF.Repositorio.Atualizar(pay);
                    }
                    else
                    {
                        itemERP = DTOupload.UpdateOrCreate(itemPDV, itemERP, ref log);
                        var result = cake.api.Save(itemERP);
                        if (result != null)
                        {

                            info += inserir ? "(novo) " : "(atualizado) " + log;
                            if (itemPDV.CodigoERP != result.id.ToString())
                            {
                                itemPDV.CodigoERP = result.id.ToString();
                                EF.Repositorio.Atualizar(itemPDV);
                            }

                            if (inserir && !erpItens.ContainsKey(result.id.Value)) // Atualiza o cache!
                                erpItens.Add(result.id.Value, result);
                        }
                        else
                        {
                            info += "ERRO " + log;
                            e++;
                        }
                    }
                    cake.AddLog($" >>> ERP {info} {itemPDV.CodigoERP} {itemPDV}");

                }
                catch (Exception ex)
                {
                    info += " !!! ERRO AO ATUALIZAR CACHE !!!";
                    ex.Data.Add("info", info);
                    ex.Data.Add("itemPDV.myID()", itemPDV.myID());
                    if (obrigatorio)
                        throw ex;
                    else
                        cake.AddLog(ex);
                }
            }

            if (i > 0 || a > 0 || e > 0)
                cake.AddLog($" >>> SYNC {typeof(Tpdv).Name} QTD: {n} Inseridos: {i} Atualizados: {a} erros: {e}\r\n");
        }
    }
}
