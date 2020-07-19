using Main.Model;
using Main.ULA;
using System;
using System.Collections;
using System.Linq;

namespace Main
{
    public class ComputerUnit
    {
        public static string InternalBus { get; set; }
        public Register InstructionRegister;
        public Register InstructionRegisterOpSource1;
        public Register InstructionRegisterOpSource2;
        public Register InstructionRegisterOpDestiny;
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
        public char CurrentInstructFormat;
        private PortSignalMapping PortMapping;
        private readonly int InternalControlPortNumberLimit = 19;
        private readonly int ExternalControlPortNumberLimit = 24;

        public ComputerUnit(UC controlUnit)
        {
            ControlUnit = controlUnit;
            Memory = new ExternalMemory();
            PortMapping = new PortSignalMapping(this);
            InstructionRegister = new Register("IR");
            InstructionRegisterOpSource1 = new Register("IR OP1");
            InstructionRegisterOpSource2 = new Register("IR OP2");
            InstructionRegisterOpDestiny = new Register("IR OP3");
            MemoryAddressRegister = new Register("MAR");
            ProgramCounter = new Register("PC");
            MemoryBufferRegister = new Register("MBR");
            S1Register = new Register("S1");
            S2Register = new Register("S2");
            S3Register = new Register("S3");
            S4Register = new Register("S4");
            ULAXRegister = new Register("X");
            ACRegister = new Register("AC");
            ULA = new ArithmeticLogicUnit(this);
        }

        public void Initialize()
        {
            PortMapping.Initialize();
        }

        public void ReceiveControlSignal(string microInstruction)
        {
            char[] instArray = microInstruction.ToCharArray();
            if (InstructionIsValid(instArray))
            {
                SendControlSignal(instArray);
                SendPortSignal(instArray);
            }
        }

        private void SendControlSignal(char[] instArray)
        {
            ULA.ReceiveOpCode(instArray[(ExternalControlPortNumberLimit+1)..(ExternalControlPortNumberLimit+4)]);
            ULA.UseZeroReg(instArray[instArray.Length-1]);
            Memory.ReceiveControlSignal((instArray[ExternalControlPortNumberLimit+4] - '0'), (instArray[ExternalControlPortNumberLimit+5] - '0'));
        }

        private void SendPortSignal(char[] instArray)
        {
            //Write First
            ReadOrWriteIntoBus(instArray, 0, InternalControlPortNumberLimit);
            //Read After
            ReadOrWriteIntoBus(instArray, 1, InternalControlPortNumberLimit);

            ReadAndWriteIntoExternalBus(instArray);
        }

        private void ReadAndWriteIntoExternalBus(char[] instArray)
        {

            //Write First
            ReadOrWriteIntoBus(instArray, InternalControlPortNumberLimit+1, ExternalControlPortNumberLimit);

            //Read After
            ReadOrWriteIntoBus(instArray, InternalControlPortNumberLimit+2, ExternalControlPortNumberLimit);
        }

        private void ReadOrWriteIntoBus(char[] instArray, int startingIndex, int endIndex)
        {
            for (int i = startingIndex; i <= endIndex; i += 2)
            {
                if (instArray[i] - '0' != 0 )
                {
                    PortMapping.delegateRegisterMapping[i]();
                }
            }
        }


        // Saber dividir o conteúdo dos Operadores dependento do format da operação
        public void GetInstructionFromInternalBus()
        {
            InstructionRegister.GetValueFromIBus();
            SetIROperators(InstructionRegister.GetValue().Substring(0, 6));
            FeedControlUnit();
        }

        private void SetIROperators(string opCode)
        {
            if (opCode.Contains('1'))
            {
                var operation = (OperationEnum) opCode.GetBitArrayFromString().GetIntFromBitArray();
                if (operation == OperationEnum.J)
                {
                    InstructionRegisterOpDestiny.SetValue(InstructionRegister.GetValue().Substring(6, 26));
                }
                else
                {
                    CurrentInstructFormat = 'I';
                    InstructionRegisterOpDestiny.SetValue(InstructionRegister.GetValue().Substring(6, 5));
                    InstructionRegisterOpSource1.SetValue(InstructionRegister.GetValue().Substring(11, 5));
                    InstructionRegisterOpSource2.SetValue("0000000000000000"+ InstructionRegister.GetValue().Substring(16, 16));
                }
            }
            else
            {
                CurrentInstructFormat = 'R';
                InstructionRegisterOpDestiny.SetValue(InstructionRegister.GetValue().Substring(6, 5));
                InstructionRegisterOpSource1.SetValue(InstructionRegister.GetValue().Substring(11, 5));
                InstructionRegisterOpSource2.SetValue(InstructionRegister.GetValue().Substring(16, 5));
            }
        }

        public void FeedControlUnit()
        {
            ControlUnit.ReceiveInstructionRegister(InstructionRegister.GetValue());
        }

        public void GetDataFromExternalBus()
        {
            MemoryBufferRegister.SetValue(ExternalMemory.ExternalBus);
        }

        public void SetDataIntoExternalBus()
        {
            ExternalMemory.ExternalBus = MemoryBufferRegister.GetValue().GetBitArrayFromString();
        }

        public void SetAddressIntoExternalBus()
        { 
            ExternalMemory.ExternalBus = MemoryAddressRegister.GetValue().GetBitArrayFromString();
        }

        private bool InstructionIsValid(char[] instArray)
        {
            return (CheckForInternalPortIndex(instArray) &&
                CheckForExternalPortIndex(instArray));
        }

        private bool CheckForInternalPortIndex(char[] instArray)
        {
            int busWrittingIndex = -1;
            for (int i = 0; i <= InternalControlPortNumberLimit; i += 2)
            {
                if (instArray[i] - '0' != 0)
                {
                    if (busWrittingIndex != -1)
                    {
                        return false;
                    }
                    busWrittingIndex = i;
                }
            }

            return true;
        }
        private bool CheckForExternalPortIndex(char[] instArray)
        {
            if( (instArray[20] - '0') + (instArray[22] - '0')+ (instArray[24] - '0') > 1)
            {
                return false;
            }
            return true;
        }

    }
}