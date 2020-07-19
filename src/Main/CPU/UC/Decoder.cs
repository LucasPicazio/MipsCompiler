using System;
using System.Collections.Generic;
using System.Text;

namespace Main.CPU.UC
{
    public class Decoder
    {
        public Instruction CBR { get; set; }
        public ComputerUnit Cpu { get; set; }
        public Decoder(Instruction cbr, ComputerUnit cpu)
        {
            CBR = cbr;
            Cpu = cpu;
        }

        public void SendControlSignal(Instruction cbr)
        {
            CBR = cbr;
            if (CBR.DecodeRegisterSource1 || CBR.DecodeRegisterSource2 || CBR.DecodeRegisterDestiny)
            {
                SetControlToInstructionFormat(CBR.ControlSignalInstruction);
            }
            Cpu.ReceiveControlSignal(CBR.ControlSignalInstruction);
        }

        private void SetControlToInstructionFormat(string instruct)
        {
            var instructCharArray = instruct.ToCharArray();
            switch (Cpu.CurrentInstructFormat)
            {
                // Register Format
                case 'R':
                    SetRegisterFormat(instructCharArray);
                    break;
                // Imediate Format
                case 'I':
                    SetImediateFormat(instructCharArray);
                    break;
            }
        }

        private void SetRegisterFormat(char[] instruct)
        {
            if (CBR.DecodeRegisterSource1)
            {
                var firstSourceOperator = Cpu.InstructionRegisterOpSource1.GetValue().GetBitArrayFromString().GetIntFromBitArray();
                if (firstSourceOperator >= 4 && firstSourceOperator <= 10 || firstSourceOperator == 36)
                {
                    instruct[firstSourceOperator] = '1';
                }
            }
            if (CBR.DecodeRegisterSource2)
            {
                var secondSourceOperator = Cpu.InstructionRegisterOpSource2.GetValue().GetBitArrayFromString().GetIntFromBitArray();
                if ((secondSourceOperator >= 4 && secondSourceOperator <= 10) || secondSourceOperator == 36)
                {
                    instruct[secondSourceOperator] = '1';
                }
            }
            if (CBR.DecodeRegisterDestiny)
            {
                var destinyOperator = Cpu.InstructionRegisterOpDestiny.GetValue().GetBitArrayFromString().GetIntFromBitArray();
                if (destinyOperator >= 4 && destinyOperator <= 10)
                {
                    instruct[destinyOperator + 1] = '1';
                }
            }

            CBR.SetControlSignalInstruction(instruct);
        }

        private void SetImediateFormat(char[] instruct)
        {
            if (CBR.DecodeRegisterSource1)
            {
                var firstSourceOperator = Cpu.InstructionRegisterOpSource1.GetValue().GetBitArrayFromString().GetIntFromBitArray();
                if (firstSourceOperator >= 4 && firstSourceOperator <= 10)
                {
                    instruct[firstSourceOperator] = '1';
                }
            }
            if (CBR.DecodeRegisterSource2)
            {
                var secondSourceOperator = Cpu.InstructionRegisterOpSource2.GetValue().GetBitArrayFromString().GetIntFromBitArray();
                if (secondSourceOperator >= 4 && secondSourceOperator <= 10)
                {
                    instruct[secondSourceOperator] = '1';
                }
            }
            CBR.SetControlSignalInstruction(instruct);
        }
    }
}
