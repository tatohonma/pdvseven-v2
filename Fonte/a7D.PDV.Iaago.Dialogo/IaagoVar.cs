using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace a7D.PDV.Iaago.Dialogo
{
    public delegate void SaveVar(string chave, string valor);

    public class IaagoVars
    {
        private readonly Random rnd = new Random();

        public IaagoVars()
        {
            Itens = new List<IaagoKeyValue>();
        }

        public bool Debug { get; set; }

        public string Text { get; set; }

        public string Intencao { get; set; }

        public string RetornoIntencao { get; set; }

        public string RetornoAtribuir { get; set; }

        public bool? IgnorarMensagemIntencao { get; set; }

        public List<IaagoKeyValue> Itens { get; set; }

        public object this[string key] => Itens.FirstOrDefault(kv => key.Equals(kv.Key, StringComparison.CurrentCultureIgnoreCase))?.Value;

        public IEnumerable<string> Keys => Itens.Select(i => i.Key);

        public int UserID()
        {
            var userId = Itens.FirstOrDefault(kv => kv.Key == "_userId");
            return userId == null ? 0 : (int)userId.Value;
        }

        public event SaveVar onAdd;

        public void Add(string key, object originalValue, bool interpretarString, DateTime? dt = null)
        {
            object value;
            if (originalValue == null)
            {
                value = null;
            }
            else if (interpretarString)
            {
                value = InterpretaValor(originalValue.ToString());
                try
                {
                    if (!key.StartsWith("@") && !key.StartsWith("_"))
                    {
                        onAdd?.Invoke(key, value.ToString());
                    }
                }
                catch
                {
                }
            }
            else if (originalValue is string str)
            {
                value = str;
                onAdd?.Invoke(key, str);
            }
            else
            {
                value = originalValue;
            }

            // TODO: Avaliar depois a data, contextos, etc...
            var pos = Itens.FindIndex(kv => key.Equals(kv.Key, StringComparison.CurrentCultureIgnoreCase));
            var nova = new IaagoKeyValue(key, value, originalValue?.ToString(), dt);
            if (value == null)
            {
                if (pos > 0)
                {
                    Itens.RemoveAt(pos);
                }
            }
            else if (pos < 0)
            {
                Itens.Add(nova);
            }
            else
            {
                Itens[pos] = nova;
            }
        }

        internal void Clear()
        {
            Itens.Clear();
        }

        public object InterpretaValor(string keyValue)
        {
            if (keyValue == null)
            {
                return null;
            }

            keyValue = keyValue.Trim();
            if (keyValue.StartsWith("'") && keyValue.EndsWith("'"))
            {
                return keyValue.Substring(1, keyValue.Length - 2);
            }
            else if (keyValue.Equals("@text", StringComparison.CurrentCultureIgnoreCase))
            {
                return this.Text;
            }
            else if (keyValue.Contains("@"))
            {
                return Interpretador.Calcula(keyValue, key =>
                {

                    int i = key.IndexOf(".");
                    string topKey;

                    if (i > 0)
                    {
                        topKey = key.Substring(1, i - 1);
                    }
                    else
                    {
                        topKey = key.Substring(1);
                    }

                    var re = new Regex(@"(\[(\d+)\])");
                    var m = re.Match(topKey);
                    int pos = -1;
                    if (m.Success)
                    {
                        pos = int.Parse(m.Groups[2].Value);
                        topKey = topKey.Replace(m.Groups[1].Value, "");
                    }

                    var value = this[topKey];
                    if (value == null)
                    {
                        return null;
                    }
                    else if (i == -1)
                    {
                        return value;
                    }

                    var subkey = key.Substring(i + 1).ToLower();
                    if (pos >= 0 && value is List<object> adic)
                    {
                        value = pos < adic.Count ? adic[pos] : null;
                        if (value is DictionaryObject odic)
                        {
                            value = odic.GetValueByKey(subkey);
                        }
                    }
                    else if (i > 0 && value is DictionaryObject dic)
                    {
                        if (pos >= 0 && dic.Count == 1 && dic.Keys.First() == "itens")
                        {
                            var dic2 = (DictionaryObject)((List<object>)dic["itens"])[pos];
                            value = dic2.GetValueByKey(subkey);
                        }
                        else
                        {
                            value = dic.GetValueByKey(subkey);
                        }
                    }
                    else
                    {
                        value = null;
                    }

                    return value;
                });
            }
            else
            {
                return Interpretador.SimpleVar(keyValue);
            }
        }

        internal void Remove(string key)
        {
            var p = Itens.FindIndex(i => i.Key == key);
            if (p >= 0)
            {
                Itens.RemoveAt(p);
            }
        }

        public string PreencheMensagem(object mensagem)
        {
            string final;
            if (mensagem is string msg)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    return string.Empty;
                }

                final = msg;
            }
            else if (mensagem is JArray msgs)
            {
                int m = rnd.Next(msgs.Count);
                final = msgs[m].Value<string>();
            }
            else
            {
                return string.Empty;
            }

            return Interpretador.FormataMensagem(final, key => this.InterpretaValor(key));
        }

        public override string ToString() => $"{Text} ({Itens.Count})";
    }
}
