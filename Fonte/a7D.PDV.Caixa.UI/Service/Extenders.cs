using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public static class Extenders
    {
        public static void LoadLocationSize(this Form frm, EConfig config, Action<string[]> extraLoad = null)
        {
            try
            {
                string valor = ConfiguracaoBD.ValorOuPadrao(config, ETipoPDV.CAIXA, AC.PDV.IDPDV.Value);
                if (!string.IsNullOrEmpty(valor))
                {
                    string[] prms = valor.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var location = new Point(int.Parse(prms[0]), int.Parse(prms[1]));
                    var size = new Size(int.Parse(prms[2]), int.Parse(prms[3]));

                    bool ok = false;
                    foreach (var s in Screen.AllScreens)
                    {
                        // Verifica se as posições são minimamente validas nas telas disponíveis
                        if (s.Bounds.Left <= location.X
                         && s.Bounds.Right >= location.X + size.Width
                         && s.Bounds.Top <= location.Y
                         && s.Bounds.Bottom >= location.Y + size.Height)
                        {
                            ok = true;
                            break;
                        }
                    }

                    if (!ok)
                        return;

                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = location;
                    frm.Size = size;
                    extraLoad?.Invoke(prms);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void SaveLocationSize(this Form frm, EConfig config, Func<string> extraSave = null)
        {
            try
            {
                string extra = extraSave?.Invoke();
                if (string.IsNullOrEmpty(extra))
                    extra = string.Empty;
                else
                    extra = "," + extra;

                string valor = $"{frm.Location.X},{frm.Location.Y},{frm.Size.Width},{frm.Size.Height}{extra}";
                ConfiguracaoBD.DefinirValorPadraoTipoPDV(config, ETipoPDV.CAIXA, AC.PDV.IDPDV.Value, valor);
            }
            catch (Exception)
            {
            }
        }
    }
}
