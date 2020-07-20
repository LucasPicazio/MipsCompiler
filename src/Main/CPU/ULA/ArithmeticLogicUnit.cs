using System;

namespace Main.ULA
{
    public class ArithmeticLogicUnit
    {
        private readonly string binaryRepresentationOfOne;
        private readonly string binaryRepresentationOfZero;
        private string opCode;
        private bool UseZero;
        private readonly OpCodeMapping operationMapping;
        private readonly ComputerUnit _CPU;
        private Tuple<int, int> mapOutput;
        private Tuple<int, int, int> mapInput;
        public bool ZeroFlag;
        public bool SignalFlag;

        public ArithmeticLogicUnit(ComputerUnit CPU, Interface view)
        {
            binaryRepresentationOfOne = "00000000000000000000000000000001";
            binaryRepresentationOfZero = "00000000000000000000000000000000";
            operationMapping = new OpCodeMapping(this);
            _CPU = CPU;
          
        }

        public void ReceiveOpCode(char[] value)
        {
            opCode = new string(value);
        }

        public void UseZeroReg(char v)
        {
            UseZero = (v == 1);
        }

        public void GetValueFromIBus()
        {
            ZeroFlag = false;
            SignalFlag = false;
            operationMapping.delegateOperationMapping[opCode]();
        }

        public void Increment()
        {
            int[] result = GetSumResult(ComputerUnit.InternalBus, binaryRepresentationOfOne);
            UpdateFlags(result);
            _CPU.ACRegister.SetValue(result);
        }

        public void Sum()
        {
            int[] result = UseZero ? GetSumResult(ComputerUnit.InternalBus, binaryRepresentationOfZero)
                                    :GetSumResult(ComputerUnit.InternalBus, _CPU.ULAXRegister.GetValue());
            UpdateFlags(result);
            _CPU.ACRegister.SetValue(result);
        }

        public void Subtract()
        {
            int[] SecondOpValue = GetComplementOfTwo(_CPU.ULAXRegister.GetValue());
            int[] result = GetSumResult(ComputerUnit.InternalBus, string.Join("", SecondOpValue));
            UpdateFlags(result);
            _CPU.ACRegister.SetValue(result);            
        }

        public void Compare()
        {
            if (!_CPU.ULAXRegister.GetValue().Contains('1') && !ComputerUnit.InternalBus.Contains('1'))
            {
                SignalFlag = false;
                ZeroFlag = true;
                _CPU.ACRegister.SetValue(binaryRepresentationOfZero);
            }
            else {
                int[] SecondOpValue = GetComplementOfTwo(_CPU.ULAXRegister.GetValue());
                int[] result = GetSumResult(ComputerUnit.InternalBus, string.Join("", SecondOpValue));
                UpdateFlags(result);
                string finalRes;
                if (SignalFlag)
                {
                    finalRes = binaryRepresentationOfOne;
                }
                else
                {
                    finalRes = binaryRepresentationOfZero;
                }
                _CPU.ACRegister.SetValue(finalRes);
            }
        }

        public void OR()
        {

            int[] firstOp = GetIntArrayFromString(ComputerUnit.InternalBus);
            int[] secondOP = GetIntArrayFromString(_CPU.ULAXRegister.GetValue());
            int[] result = new int[firstOp.Length];

            for (int i = 0; i < firstOp.Length; i++)
            {
                if(firstOp[i] + secondOP[i] >= 1)
                {
                    result[i] = 1;
                }
            }
            _CPU.ACRegister.SetValue(result);
        }

        public void UpdateFlags(int[] result)
        {
            SignalFlag = (result[0] == 1);

            for (int i = 0; i < result.Length; i++)
            {
                if(result[i] == 1)
                {
                    ZeroFlag = false;
                    return;
                }
            }
            ZeroFlag = true;
        }

        private int[] GetSumResult(string firtOp, string secondOp)
        {
            int[] firstOpArray = GetIntArrayFromString(firtOp);
            int[] secondOpArray = GetIntArrayFromString(secondOp);            
            mapOutput = Tuple.Create(0, 0);

            int[] result = new int[firstOpArray.Length];
            for (int i = firstOpArray.Length - 1; i >= 0; i--)
            {
                mapInput = Tuple.Create(mapOutput.Item2, firstOpArray[i], secondOpArray[i]);
                if (operationMapping.BitSumResultMap.TryGetValue(mapInput, out mapOutput))
                {
                    result[i] = mapOutput.Item1;
                }
            }

            if (mapOutput.Item1 != firstOpArray[0] && (firstOpArray[0] == secondOpArray[0]))
            {
                throw new OverflowException("Overflow na operação de soma, a soma resultante dos valores inseridos não pode ser representada pros dados bits");
            }

            return result;
        }


        private int[] GetComplementOfTwo(string value)
        {
            int[] valueIntArray = GetIntArrayFromString(value);
            
            for(int i = 0; i < valueIntArray.Length; i++)
            {
                if(valueIntArray[i] == 0)
                {
                    valueIntArray[i] = 1;
                }
                else
                {
                    valueIntArray[i] = 0;
                }
            }

            return valueIntArray;
        }

        private int[] GetIntArrayFromString(string value)
        {
            char[] a = value.ToCharArray();
            int[] result = new int[a.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = a[i] - '0';
            }

            return result;
        }
    }
}