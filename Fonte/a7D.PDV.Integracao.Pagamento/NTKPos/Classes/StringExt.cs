namespace a7D.PDV.Integracao.Pagamento.NTKPos
{
    public static class StringExt
    {
        public static string PadBoth(this string str, int length = 20, char character = ' ')
        {
            if (str == null) return "";
            return str.PadLeft((length - str.Length) / 2 + str.Length, character).PadRight(length, character);
        }

        public static string Max(this string str, int length = 20)
        {
            if (str.Length > length)
                return str.Substring(0, length);
            else
                return str;
        }
    }
}
