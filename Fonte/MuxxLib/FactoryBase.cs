using MuxxLib.Enum_s;
using MuxxLib.Estruturas;
using MuxxLib.Interface;
using MuxxLib.Operações;
using System;

namespace MuxxLib
{
    public abstract class FactoryBase
    {
        public abstract object Modal { get; set; }

        public ITipoDeDado Operacao(PW_GetData[] getData, ushort indice)
        {
            switch (getData[indice].bTipoDeDado)
            {
                case ((byte)PWDAT.MENU):
                    return Menu(getData[indice]);

                case ((byte)PWDAT.USERAUTH):
                    return LeituraDados(getData[indice], true);

                case ((byte)PWDAT.TYPED):
                    return LeituraDados(getData[indice], false);

                case ((byte)PWDAT.PPREMCRD):
                    Interop.PW_iPPRemoveCard();
                    return new PPRemCRD();

                case ((byte)PWDAT.CARDINF):
                    if (getData[0].ulTipoEntradaCartao != 1)
                    {
                        Interop.PW_iPPGetCard(indice);
                        return new CardInfo();
                    }
                    else
                        return null;

                case ((byte)PWDAT.CARDOFF):
                    Interop.PW_iPPGoOnChip(indice);
                    return new CardOff();

                case ((byte)PWDAT.CARDONL):
                    Interop.PW_iPPFinishChip(indice);
                    return new CardONL();

                case ((byte)PWDAT.PPCONF):
                    Interop.PW_iPPConfirmData(indice);
                    return new PPConf();

                case ((byte)PWDAT.PPDATAPOSCNF):
                    Interop.PW_iPPPositiveConfirmation(indice);
                    return new PPDataPosCNF();

                case ((byte)PWDAT.PPENCPIN):
                    Interop.PW_iPPGetPIN(indice);
                    return new PPEncPin();

                case ((byte)PWDAT.PPGENCMD):
                    Interop.PW_iPPGenericCMD(indice);
                    return new PPGenCMD();

                default:
                    throw new Exception("Tipo de dado desconhecido: " + getData[indice].bTipoDeDado);
            }
        }

        protected abstract ITipoDeDado LeituraDados(PW_GetData pW_GetData, bool hide);

        protected abstract ITipoDeDado Menu(PW_GetData pW_GetData);
    }
}
