using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;

namespace a7D.PDV.BLL
{
    public class Mesa
    {
        public static List<MesaInformation> Listar()
        {
            List<object> listaObj = CRUD.Listar(new MesaInformation());
            List<MesaInformation> lista = listaObj.ConvertAll(new Converter<object, MesaInformation>(MesaInformation.ConverterObjeto));

            return lista;
        }

        public static MesaInformation Carregar(Int32 idMesa)
        {
            MesaInformation obj = new MesaInformation { IDMesa = idMesa };
            obj = (MesaInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static MesaInformation CarregarPorGUID(String guidIdentificacao)
        {
            MesaInformation obj = new MesaInformation();

            if (guidIdentificacao != null)
            {
                obj = new MesaInformation { GUIDIdentificacao = guidIdentificacao };
                obj = (MesaInformation)CRUD.Carregar(obj);
            }

            return obj;
        }

        public static MesaInformation CarregarPorNumero(Int32 numero)
        {
            MesaInformation obj = new MesaInformation { Numero = numero };
            obj = (MesaInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(MesaInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idMesa)
        {
            try
            {
                MesaInformation obj = new MesaInformation { IDMesa = idMesa };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }

        public static MesaInformation Validar(Int32 numero, bool pagamento = false)
        {
            MesaInformation mesa = Mesa.CarregarPorNumero(numero);

            if (mesa.IDMesa == null)
            {
                throw new ExceptionPDV(CodigoErro.A301);
            }
            else if (mesa.StatusMesa.IDStatusMesa == (int)EStatusMesa.ContaSolicitada && !pagamento)
            {
                throw new ExceptionPDV(CodigoErro.A302);
            }
            else if (mesa.StatusMesa.IDStatusMesa == (int)EStatusMesa.Reservada)
            {
                throw new ExceptionPDV(CodigoErro.A303);
            }
            else
            {
                return mesa;
            }
        }

        public static void AlterarStatus(String guidIdentificacao, EStatusMesa eStatusMesa)
        {
            MesaInformation mesa = CarregarPorGUID(guidIdentificacao);
            mesa.StatusMesa.IDStatusMesa = (int)eStatusMesa;

            Mesa.Salvar(mesa);
        }

        //[Serializable]
        //public class MesaException : Exception
        //{
        //    public int Codigo { get; }
        //    public MesaException(int codigo) { Codigo = codigo; }
        //    public MesaException(int codigo, string message) : base(message) { Codigo = codigo; }
        //    public MesaException(int codigo, string message, Exception inner) : base(message, inner) { Codigo = codigo; }
        //    protected MesaException(
        //      int codigo,
        //      System.Runtime.Serialization.SerializationInfo info,
        //      System.Runtime.Serialization.StreamingContext context) : base(info, context) { Codigo = codigo; }
        //}
    }
}
