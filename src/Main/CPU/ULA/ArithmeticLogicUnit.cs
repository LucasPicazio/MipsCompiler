using System;

namespace Main.ULA
{
    public class ArithmeticLogicUnit
    {
        private readonly string binaryRepresentationOfOne;
        private string opCode;
        private readonly OpCodeMapping operationMapping;
        private readonly ComputerUnit _CPU;
        private Tuple<int, int> mapOutput;
        private Tuple<int, int, int> mapInput;
        public bool ZeroFlag;
        public bool SignalFlag;

        public ArithmeticLogicUnit(ComputerUnit CPU)
        {
            binaryRepresentationOfOne = "00000000001";
            operationMapping = new OpCodeMapping(this);
            _CPU = CPU;
        }

        public void ReceiveOpCode(string value)
        {
            opCode = value;
        }

        public void GetValueFromIBus()
        {
            operationMapping.delegateOperationMapping[opCode]();
        }

        public void Increment()
        {
            int[] result = GetSumResult(ComputerUnit.InternalBus, binaryRepresentationOfOne);
            _CPU.ACRegister.SetValue(result);
        }

        public void Sum()
        {
            int[] result = GetSumResult(ComputerUnit.InternalBus, _CPU.ULAXRegister.GetValue());
            _CPU.ACRegister.SetValue(result);
        }

        public void Subtract()
        {
            int[] SecondOpValue = GetComplementOfTwo(_CPU.ULAXRegister.GetValue());
            int[] result = GetSumResult(ComputerUnit.InternalBus, string.Join("", SecondOpValue));
            _CPU.ACRegister.SetValue(result);
        }

        public void Compare()
        {
            ZeroFlag = false;
            SignalFlag = false;
            int[] SecondOpValue = GetComplementOfTwo(_CPU.ULAXRegister.GetValue());
            int[] result = GetSumResult(ComputerUnit.InternalBus, string.Join("", SecondOpValue));
            SignalFlag = (result[0] == 1);

            foreach (int i in result)
            {
                if (!ZeroFlag)
                {
                    ZeroFlag = (result[i] == 1);
                }
                else
                {
                    return;
                }
            }
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
            
            for(int i = 1; i < valueIntArray.Length; i++)
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