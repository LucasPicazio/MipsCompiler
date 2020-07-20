using Main.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Main
{
    public class ExternalMemory
    {
        public static BitArray ExternalBus { get; set; }
        public static List<Command> MemoryAddress { get; set; }
        public static int _targetAddress { get; set; }
        public static bool EndOfProgram { get; set; }
        private int ReadSignal { get; set; }
        private int WriteSignal { get; set; }

        public ExternalMemory()
        {
            ExternalBus = new BitArray(32);
            MemoryAddress = new List<Command>( (int) Math.Pow(2, 16));
        }

        public void ReceiveAssemblyProgram(List<Command> binaryAssembly)
        {
            MemoryAddress.AddRange(binaryAssembly);
        }

        public void ReceiveControlSignal(int readSign = 0, int writeSign = 0)
        {
            ReadSignal = readSign;
            WriteSignal = writeSign;
        }

        public void GetValueFromExternalBus()
        {
            if(ReadSignal + WriteSignal == 0)
            {
                GetAddressValueFromBus();
            }
            else if (WriteSignal > ReadSignal)
            {
                GetDataValueFromBus();
            }
        }

        public void SetDataIntoExternalBus()
        {
            if (ReadSignal > WriteSignal)
            {
                InsertDataValueIntoBus();
            }
        }

        private void InsertDataValueIntoBus()
        {
            if (_targetAddress >= MemoryAddress.Count)
            {
                ExternalBus = new BitArray(32);
            }
            ExternalBus = MemoryAddress[_targetAddress].Bits;
        }

        private void GetDataValueFromBus()
        {
            MemoryAddress[_targetAddress].Bits = ExternalBus;
        }

        private void GetAddressValueFromBus()
        {
            _targetAddress = ExternalBus.GetIntFromBitArray();
        }

        public static Command GetActualCommand()
        {
            if (_targetAddress >= 0 && _targetAddress < MemoryAddress.Count)
                return MemoryAddress[_targetAddress];
            else
                return default;
        }

    }
}
