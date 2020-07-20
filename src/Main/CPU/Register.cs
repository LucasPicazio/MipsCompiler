using System;
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

        public string GetReversedValue()
        {
            var temp = _value.ToCharArray();
            Array.Reverse(temp);
            return new string(temp);
        }

        public void SetValue(string value)
        {
            _value = value;
        }

        public void SetValue(int[] value)
        {
            _value = string.Join("", value);
        }

        public void SetReversedValue(BitArray value)
        {
            var temp = value.GetStringFromBitArray().ToCharArray();
            Array.Reverse(temp);
            _value = new string(temp);
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
