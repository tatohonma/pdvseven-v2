using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuxxLib.Interface
{
   public interface ITipoDeDado
   {
      short Operacao(ushort i, ref string message);
   }
}
