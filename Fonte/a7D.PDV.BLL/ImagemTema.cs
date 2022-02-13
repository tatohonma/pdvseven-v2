using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;

namespace a7D.PDV.Model.BLL
{
    public class ImagemTema
    {
        public static List<ImagemTemaInformation> Listar()
        {
            List<object> listObj = CRUD.Listar(new ImagemTemaInformation());
            List<ImagemTemaInformation> list = listObj.Cast<ImagemTemaInformation>().ToList();

            return list;
        }

        public static List<ImagemTemaInformation> ListarCompleto()
        {
            var lista = Listar();
            foreach (var item in lista)
            {
                item.Idioma = Idioma.Carregar(item.Idioma.IDIdioma.Value);
                item.TemaCardapio = TemaCardapio.Carregar(item.TemaCardapio.IDTemaCardapio.Value);
            }
            return lista;
        }

        public static List<ImagemTemaInformation> ListarPorTema(int idTemaCardapio)
        {
            List<object> listObj = CRUD.Listar(new ImagemTemaInformation { TemaCardapio = new TemaCardapioInformation { IDTemaCardapio = idTemaCardapio } });
            List<ImagemTemaInformation> list = listObj.Cast<ImagemTemaInformation>().ToList();

            foreach (var item in list)
                item.Idioma = Idioma.Carregar(item.Idioma.IDIdioma.Value);

            return list;
        }

        public static ImagemTemaInformation CarregarPorNome(string nome, int idTemaCardapio, int idIdioma)
        {
            ImagemTemaInformation obj = new ImagemTemaInformation
            {
                Nome = nome,
                TemaCardapio = new TemaCardapioInformation { IDTemaCardapio = idTemaCardapio },
                Idioma = new IdiomaInformation { IDIdioma = idIdioma }
            };
            return (ImagemTemaInformation)CRUD.Carregar(obj);
        }

        public static ImagemTemaInformation Carregar(int idImagemTema)
        {
            ImagemTemaInformation obj = new ImagemTemaInformation { IDImagemTema = idImagemTema };
            return (ImagemTemaInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(ImagemTemaInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(ImagemTemaInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(ImagemTemaInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(ImagemTemaInformation imagemTema)
        {
            CRUD.Excluir(imagemTema);
        }

        public static void Excluir(int idImagemTema)
        {
            Excluir(new ImagemTemaInformation { IDImagemTema = idImagemTema });
        }
    }
}

