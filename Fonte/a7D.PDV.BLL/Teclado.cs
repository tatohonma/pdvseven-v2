using System;
using System.Runtime.InteropServices;

namespace a7D.PDV.BLL
{
    public class Teclado
    {
        public partial class NativeMethods
        {
            [DllImport("user32.dll", EntryPoint = "BlockInput")]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);
        }

        public static void BlockInput(Boolean block)
        {
            NativeMethods.BlockInput(block);
        }
    }
}
