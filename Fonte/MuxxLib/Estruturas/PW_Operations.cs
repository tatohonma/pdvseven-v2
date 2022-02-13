using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MuxxLib.Estruturas
{
   [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
   public struct PW_Operations
   {
      byte bOperType;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
      string szText;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
      string szValue;
   }
}
