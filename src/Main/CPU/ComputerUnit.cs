﻿using Main.ULA;
using System;
using System.Collections;
using System.Linq;

namespace Main
{
    public class ComputerUnit
    {
        public static string InternalBus { get; set; }
        public Register InstructionRegister;
        public Register InstructionRegisterOp1;
        public Register InstructionRegisterOp2;
        public Register InstructionRegisterOp3;
        public Register MemoryAddressRegister;
        public Register ProgramCounter;
        public Register MemoryBufferRegister;
        public Register S1Register;
        public Register S2Register;
        public Register S3Register;
        public Register S4Register;
        public Register ULAXRegister;
        public Register ACRegister;

        public ArithmeticLogicUnit ULA;
        public ExternalMemory Memory;
        public readonly UC ControlUnit;
        private PortSignalMapping PortMapping;
        private readonly int InternalControlPortNumber = 20;

        public ComputerUnit(UC controlUnit)
        {
            ControlUnit = controlUnit;
        }

        public void Initialize()
        {
            InstructionRegister = new Register();
            MemoryAddressRegister = new Register();
            ProgramCounter = new Register();
            MemoryBufferRegister = new Register();
            S1Register = new Register();
            S2Register = new Register();
            S3Register = new Register();
            S4Register = new Register();
            ULAXRegister = new Register();
            ACRegister = new Register();
            Memory = new ExternalMemory();
            PortMapping = new PortSignalMapping(this);
            ULA = new ArithmeticLogicUnit(this);
        }

        public void ReceiveControlSignal(string microInstruction)
        {
            char[] instArray = microInstruction.ToCharArray();
            if (InstructionIsValid(instArray))
            {
                SendPortSignal(instArray);
            }
        }

        private void SendPortSignal(char[] instArray)
        {
            //Write
            ReadOrWriteIntoBus(instArray, 0);
            //Read
            ReadOrWriteIntoBus(instArray, 1);
        }

        private void ReadOrWriteIntoBus(char[] instArray, int startingIndex)
        {
            for (int i = startingIndex; i < InternalControlPortNumber; i += 2)
            {
                if (instArray[i] - '0' != 0)
                {
                    PortMapping.delegateRegisterMapping[i]();
                }
            }
        }

        public void FeedControlUnit()
        {
            ControlUnit.ReceiveInstructionRegister(InstructionRegister.GetValue());
        }

        // Saber dividir o conteúdo dos Operadores dependento do format da operação
        public void GetInstructionFromInternalBus()
        {
            InstructionRegister.GetValueFromIBus();
            InstructionRegisterOp1.SetValue(InstructionRegister.GetValue().Substring(6, 5));
            InstructionRegisterOp2.SetValue(InstructionRegister.GetValue().Substring(11, 5));
            InstructionRegisterOp3.SetValue(InstructionRegister.GetValue().Substring(16, 5));
            FeedControlUnit();
        }

        public void GetDataFromExternalBus()
        {
            MemoryBufferRegister.SetValue(ExternalMemory.ExternalBus.ToString());
        }

        public void SetDataIntoExternalBus()
        {
            ExternalMemory.ExternalBus = GetBitArrayFromString(MemoryBufferRegister.GetValue());
        }

        public void SetAddressIntoExternalBus()
        { 
            ExternalMemory.ExternalBus = GetBitArrayFromString(MemoryAddressRegister.GetValue());
        }

        private bool InstructionIsValid(char[] instArray)
        {
            return (CheckForInternalPortIndex(instArray) &&
                CheckForExternalPortIndex(instArray));
        }

        private bool CheckForInternalPortIndex(char[] instArray)
        {
            int busWrittingIndex = -1;
            for (int i = 0; i <= InternalControlPortNumber; i += 2)
            {
                if (instArray[i] - '0' != 0 && busWrittingIndex == -1)
                {
                    busWrittingIndex = i;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        private bool CheckForExternalPortIndex(char[] instArray)
        {
            if(instArray[22] + instArray[24] + instArray[25] > 1)
            {
                return false;
            }
            return true;
        }

        private BitArray GetBitArrayFromString(string str)
        {
            return new BitArray(str.Select(c => c == '1').ToArray());
        }
    }
}