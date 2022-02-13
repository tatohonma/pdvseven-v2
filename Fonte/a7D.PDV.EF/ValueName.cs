using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace a7D.PDV.EF
{
    
    public class ValueName
    {
        public ValueName(int value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public static ValueName From(IValueName valueName)
        {
            return valueName.GetValueName();
        }

        public int Value { get; internal set; }
        public string Name { get; internal set; }

        public static TEntity[] Convert<TEntity>(Type tpEnum) where TEntity : class, IValueName, new()
        {
            var itens = new List<TEntity>();
            foreach (var nameEnum in System.Enum.GetNames(tpEnum))
            {
                var objT = new TEntity();
                if (objT is IValueName vn)
                {
                    int id = (int)System.Enum.Parse(tpEnum, nameEnum);
                    string name;

                    if (tpEnum.GetField(nameEnum).GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] atrName && atrName.Length==1)
                        name = atrName[0].Description;
                    else
                        name = nameEnum;

                    vn.SetValueName(id, name);
                    itens.Add(objT);
                }
            }
            return itens.ToArray();
        }
    }
}
