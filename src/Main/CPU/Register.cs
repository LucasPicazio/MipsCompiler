using System.Collections;
using System.Linq;

namespace Main
{
    public class Register
    {
        private string _value;
        private string RegisterName;

        public Register(string name)
        {
            RegisterName = name;
            _value = "00000000000000000000000000000000";
        }

        public string GetValue()
        {
            return _value;
        }

        public void SetValue(string value)
        {
            _value = value;
        }

        public void SetValue(int[] value)
        {
            _value = string.Join("", value);
        }

        public void SetValue(BitArray value)
        {
            _value = value.GetStringFromBitArray();
        }

        public void GetValueFromIBus()
        {
            _value = ComputerUnit.InternalBus;
        }

        public void SetValueIntoIBus()
        {
            ComputerUnit.InternalBus = _value;
        }

    }
}
