using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuxxLib.Enum_s
{
   public enum PWDAT : byte
   {
      MENU = 1,
      TYPED,
      CARDINF,
      PPENTRY = 5,
      PPENCPIN,
      CARDOFF = 9,
      CARDONL,
      PPCONF,
      BARCODE,
      PPREMCRD,
      PPGENCMD,
      PPDATAPOSCNF = 16,
      USERAUTH
   }
}
