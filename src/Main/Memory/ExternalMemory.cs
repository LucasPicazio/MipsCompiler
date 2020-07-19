using Main.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Main
{
    public class ExternalMemory
    {
        public static BitArray ExternalBus { get; set; }
        public static List<Command> MemoryAddress { get; set; }
        public static int _targetAddress { get; set; }
        private int ReadSignal { get; set; }
        private int WriteSignal { get; set; }

        public ExternalMemory()
        {
            ExternalBus = new BitArray(32);
            MemoryAddress = new List<Command>();
        }

        public void ReceiveAssemblyProgram(List<Command> binaryAssembly)
        {
            MemoryAddress = new List<Command>(binaryAssembly);
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
            if (_targetAddress >= 0)
                return MemoryAddress[_targetAddress];
            else
                return default;
        }

    }
}
