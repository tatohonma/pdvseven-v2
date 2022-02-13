using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmConsultarStatusOperacional : Form
    {
        private string[] dados;

        public frmConsultarStatusOperacional()
        {
            InitializeComponent();
        }

        public frmConsultarStatusOperacional(string[] dados)
        {
            this.dados = dados;
            InitializeComponent();
        }

        private void frmConsultarStatusOperacional_Load(object sender, EventArgs e)
        {
            if (dados != null && dados.Length == 23)
            {
                var status =
@"---- Status ----------------------------------
 NSERIE ........... {0}
 TIPO_LAN ......... {1}
 LAN_IP ........... {2}
 LAN_MAC .......... {3}
 LAN_MASK ......... {4}
 LAN_GW ........... {5}
 LAN_DNS_1 ........ {6}
 LAN_DNS_2 ........ {7}
 STATUS_LAN ....... {8}
 NIVEL_BATERIA .... {9}
 MT_TOTAL ......... {10}
 MT_USADA ......... {11}
 DH_ATUAL ......... {12}
 VER_SB ........... {13}
 VER_LAYOUT ....... {14}
 ULTIMO_CF-E-SAT .. {15}
 LISTA_INICIAL .... {16}
 LISTA_FINAL ...... {17}
 DH_CFe ........... {18}
 DH_ULTIMA ........ {19}
 CERT_EMISSAO ..... {20}
 CERT_VENCIMENTO .. {21}
 ESTADO_OPERACAO .. {22}
----------------------------------------------";
                txtDados.Text = string.Format(status,
                    dados[0],
                    dados[1],
                    dados[2],
                    dados[3],
                    dados[4],
                    dados[5],
                    dados[6],
                    dados[7],
                    dados[8],
                    dados[9],
                    dados[10],
                    dados[11],
                    dados[12],
                    dados[13],
                    dados[14],
                    dados[15],
                    dados[16],
                    dados[17],
                    dados[18],
                    dados[19],
                    dados[20],
                    dados[21],
                    dados[22]);
            }
        }
    }
}
