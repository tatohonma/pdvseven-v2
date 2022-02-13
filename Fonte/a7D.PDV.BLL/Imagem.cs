using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.DAL;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using a7D.PDV.BLL.Utils;

namespace a7D.PDV.BLL
{
    public static class Imagem
    {
        public static ImagemInformation CriarNova(Image img)
        {
            var novo = new ImagemInformation();
            novo.AtualizarImagem(img);
            return novo;
        }

        public static void Adicionar(ImagemInformation imagem)
        {
            CRUD.Adicionar(imagem);
        }

        public static ImagemInformation Carregar(int idImagem)
        {
            var obj = new ImagemInformation { IDImagem = idImagem };
            return CRUD.Carregar(obj) as ImagemInformation;
        }

        public static ImagemInformation CarregarCompleto(int idImagem)
        {
            var imagem = Carregar(idImagem);
            CarregarDados(imagem);
            return imagem;
        }

        public static void Excluir(int idImagem)
        {
            Excluir(new ImagemInformation { IDImagem = idImagem });
        }

        public static void Excluir(ImagemInformation imagem)
        {
            CRUD.Excluir(imagem);
        }

        public static List<ImagemInformation> Listar()
        {
            var obj = new ImagemInformation();
            return CRUD.Listar(obj).Cast<ImagemInformation>().ToList();
        }

        public static void Salvar(ImagemInformation imagem)
        {
            CRUD.Salvar(imagem);
        }

        public static void CarregarDados(ImagemInformation imagem)
        {
            imagem.Dados = ImagemDAL.CarregarDados(imagem.IDImagem.Value);
        }

        public static void AtualizarImagem(this ImagemInformation imagemInformation, Image img)
        {
            var dados = new ConversorImagemParaByteArray(img).Dados;
            imagemInformation.Altura = img.Height;
            imagemInformation.Largura = img.Width;
            imagemInformation.Dados = dados;
            imagemInformation.Tamanho = dados.Length;
        }
    }
}
