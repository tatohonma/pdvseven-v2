using System.Text.RegularExpressions;

namespace a7D.PDV.BLL.Utils
{
    public static class ValidacaoEan
    {
        public static bool Validar(string codigo)
        {
            if (codigo != (new Regex("[^0-9]").Replace(codigo, "")))
                return false;

            switch (codigo.Length)
            {
                case 8:
                    codigo = "000000" + codigo;
                    break;
                case 12:
                    codigo = "00" + codigo;
                    break;
                case 13:
                    codigo = "0" + codigo;
                    break;
                case 14:
                    break;
                default:
                    return false;
            }

            int[] a = new int[13];
            a[0] = int.Parse(codigo[0].ToString()) * 3;
            a[1] = int.Parse(codigo[1].ToString()) * 3;
            a[2] = int.Parse(codigo[2].ToString()) * 3;
            a[3] = int.Parse(codigo[3].ToString()) * 3;
            a[4] = int.Parse(codigo[4].ToString()) * 3;
            a[5] = int.Parse(codigo[5].ToString()) * 3;
            a[6] = int.Parse(codigo[6].ToString()) * 3;
            a[7] = int.Parse(codigo[7].ToString()) * 3;
            a[8] = int.Parse(codigo[8].ToString()) * 3;
            a[9] = int.Parse(codigo[9].ToString()) * 3;
            a[10] = int.Parse(codigo[10].ToString()) * 3;
            a[11] = int.Parse(codigo[11].ToString()) * 3;
            a[12] = int.Parse(codigo[12].ToString()) * 3;

            int sum = a[0] + a[1] + a[2] + a[3] + a[4]
                + a[5] + a[6] + a[7] + a[8] + a[9] + a[10] + a[11] + a[12];

            int check = (10 - (sum % 10)) % 10;
            int last = int.Parse(codigo[13].ToString());

            return check == last;
        }
    }
}
