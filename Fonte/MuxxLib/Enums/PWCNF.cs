using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuxxLib.Enum_s
{
   public enum PWCNF: uint
   {
      PWCNF_CNF_AUTO = 0x00000121,
      PWCNF_CNF_MANU_AUT = 0x00003221,
      PWCNF_REV_MANU_AUT = 0x00003231,
      PWCNF_REV_PRN_AUT = 0x00013131,
      PWCNF_REV_DISP_AUT = 0x00023131,
      PWCNF_REV_COMM_AUT = 0x00033131,
      PWCNF_REV_ABORT = 0x00043131,
      PWCNF_REV_OTHER_AUT = 0x00073131,
      PWCNF_REV_PWR_AUT = 0x00083131,
      PWCNF_REV_FISC_AUT = 0x00093131
   }
}
