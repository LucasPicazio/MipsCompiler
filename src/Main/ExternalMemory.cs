using System;
using System.Collections;
using System.Collections.Generic;

namespace Main
{
    public class ExternalMemory
    {
        public static BitArray ExternalBus { get; set; }
        private static List<BitArray> MemoryAddress { get; set; }
        private int _targetAddress { get; set; }
        private int ReadSignal { get; set; }
        private int WriteSignal { get; set; }

        public ExternalMemory()
        {
            ExternalBus = new BitArray(32);
        }

        public void ReceiveAssemblyProgram(List<BitArray> binaryAssembly)
        {
            MemoryAddress = new List<BitArray>(binaryAssembly);
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
            ExternalBus = MemoryAddress[_targetAddress];
        }

        private void GetDataValueFromBus()
        {
            MemoryAddress[_targetAddress] = ExternalBus;
        }

        private void GetAddressValueFromBus()
        {
            _targetAddress = ExternalBus.GetIntFromBitArray();
        }
    }
}
