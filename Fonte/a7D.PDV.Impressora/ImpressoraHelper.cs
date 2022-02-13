using System;
using System.Linq;

namespace a7D.PDV.Impressora
{
    public static class ImpressoraHelper
    {

        private static byte[] IntTobyte(int[] data)
        {
            return data.Select(x => (byte)x).ToArray(); // coonvert int array to byte
        }

        public static void AbrirGaveta(string modeloImpressora)
        {
            try
            {
                int[] pulse = { 27, 112, 0, 100, 200 };
                RawPrinterHelper.SendBytesToPrinter(modeloImpressora, IntTobyte(pulse));
            }
            catch (Exception)
            {
            }
        }
    }
}
