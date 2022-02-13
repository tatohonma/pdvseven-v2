using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;

namespace a7D.PDV.BLL
{
    public class Usuario
    {
        public static UsuarioInformation Carregar(Int32 idUsuario)
        {
            UsuarioInformation obj = new UsuarioInformation { IDUsuario = idUsuario };
            obj = (UsuarioInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static void Salvar(UsuarioInformation obj)
        {
            #region Chave de acesso duplicado
            UsuarioInformation usuarioSenhaExistente = (UsuarioInformation)CRUD.Carregar(new UsuarioInformation { Senha = obj.Senha, Ativo = true, Excluido = false });

            if ((usuarioSenhaExistente.IDUsuario != null && usuarioSenhaExistente.IDUsuario != obj.IDUsuario) ||
                obj.Senha == "555" ||
                obj.Senha == "2606" ||
                obj.Senha == "260677" ||
                obj.Senha == "9999")
            {
                throw new ExceptionPDV(CodigoErro.E056);
            }
            #endregion

            obj.Excluido = false;
            obj.DtUltimaAlteracao = DateTime.Now;
            CRUD.Salvar(obj);
        }

        public static List<UsuarioInformation> Listar()
        {
            UsuarioInformation objFiltro = new UsuarioInformation();
            objFiltro.Excluido = false;

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<UsuarioInformation> lista = listaObj.ConvertAll(new Converter<Object, UsuarioInformation>(UsuarioInformation.ConverterObjeto));

            return lista;
        }

        public static List<UsuarioInformation> ListarExcluidos()
        {
            UsuarioInformation objFiltro = new UsuarioInformation();
            objFiltro.Excluido = true;

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<UsuarioInformation> lista = listaObj.ConvertAll(new Converter<Object, UsuarioInformation>(UsuarioInformation.ConverterObjeto));

            return lista;
        }

        public static UsuarioInformation Autenticar(String senha)
        {
            UsuarioInformation obj = new UsuarioInformation { Excluido = false, Senha = senha };
            obj = (UsuarioInformation)CRUD.Carregar(obj);

            if (obj.IDUsuario == null)
                throw new ExceptionPDV(CodigoErro.A000);
            else if (obj.Ativo == false)
                throw new ExceptionPDV( CodigoErro.A010);

            return obj;
        }

        public static void Excluir(Int32 idUsuario)
        {
            try
            {
                UsuarioInformation obj = Carregar(idUsuario);
                obj.Ativo = false;
                obj.Excluido = true;
                obj.DtUltimaAlteracao = DateTime.Now;

                CRUD.Salvar(obj);
            }
            catch (Exception)
            {
                throw new ExceptionPDV(CodigoErro.EF9A);
            }
        }
    }
}
