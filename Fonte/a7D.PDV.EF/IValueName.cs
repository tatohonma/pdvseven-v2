using System;

namespace a7D.PDV.EF
{
    public interface IValueName
    {
        ValueName GetValueName();
        void SetValueName(int value, string name);
    }
}
