using a7D.PDV.BLL;
using a7D.PDV.Model;
using a7D.PDV.SAT;
using a7D.PDV.SATComunicacao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace a7D.PDV.SATServiceTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var req = new SAT_SERVICE_UI.EnviarVendaRequest
            {
                CodigoDeAtivacao = "123456789",
                DadosVenda = @"<CFe><infCFe versaoDadosEnt=""0.06""><ide><CNPJ>16716114000172</CNPJ><signAC>SGR-SAT SISTEMA DE GESTAO E RETAGUARDA DO SAT</signAC><numeroCaixa>006</numeroCaixa></ide><emit><CNPJ>08723218000186</CNPJ><IE>149626224113</IE><indRatISSQN>N</indRatISSQN></emit><dest /><det nItem=""1""><prod><cProd>1006</cProd><xProd>Bata frita</xProd><NCM>21069090</NCM><CFOP>5102</CFOP><uCom>Un</uCom><qCom>1.0000</qCom><vUnCom>25.00</vUnCom><indRegra>A</indRegra></prod><imposto><ICMS><ICMSSN102><Orig>0</Orig><CSOSN>500</CSOSN></ICMSSN102></ICMS><PIS><PISSN><CST>49</CST></PISSN></PIS><COFINS><COFINSSN><CST>49</CST></COFINSSN></COFINS></imposto></det><det nItem=""2""><prod><cProd>1057</cProd><xProd>AÃ§aÃ­ com Granola e Banana</xProd><NCM>21069090</NCM><CFOP>5102</CFOP><uCom>Un</uCom><qCom>1.0000</qCom><vUnCom>17.00</vUnCom><indRegra>A</indRegra></prod><imposto><ICMS><ICMSSN102><Orig>0</Orig><CSOSN>500</CSOSN></ICMSSN102></ICMS><PIS><PISSN><CST>49</CST></PISSN></PIS><COFINS><COFINSSN><CST>49</CST></COFINSSN></COFINS></imposto></det><total><DescAcrEntr /></total><pgto><MP><cMP>01</cMP><vMP>42.00</vMP></MP></pgto></infCFe></CFe>",
                NumeroSessao = 124512
            };

            textBox1.Text = new SAT_SERVICE_UI.SATClient().EnviarVenda(req).Retorno;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = new SAT_SERVICE_UI.SATClient().Log("123456789");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var cod = new Random().Next(1, 999999);
            var ret = ComunicacaoSat.ConsultarSat(cod);
            textBox1.Text = ret;
        //    textBox1.Text += "\r\n" + StringFromNativeUtf8(NativeUtf8FromString(ret));
        }

        // https://stackoverflow.com/questions/10773440/conversion-in-net-native-utf-8-managed-string
        public static IntPtr NativeUtf8FromString(string managedString)
        {
            int len = Encoding.UTF8.GetByteCount(managedString);
            byte[] buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            return nativeUtf8;
        }

        public static string StringFromNativeUtf8(IntPtr nativeUtf8)
        {
            int len = 0;
            while (Marshal.ReadByte(nativeUtf8, len) != 0) ++len;
            byte[] buffer = new byte[len];
            Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

    }
}
