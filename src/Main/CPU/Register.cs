namespace Main
{
    public class Register
    {
        private string _value;

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
