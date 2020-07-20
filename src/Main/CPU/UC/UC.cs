using Main.CPU;
using Main.CPU.UC;
using Main.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Main
{
    public class UC
    {
        // SeqLogic > CAR (Address) > firmware > CBR (Command) > SeqLogic
        public Interface View { get; internal set; }
        private ComputerUnit Cpu { get; set; }
        private CycleDictionary Firmware;
        private CyclePointer CAR;
        private Instruction CBR;
        private Decoder InstructionDecoder;
        private string NextInstruction;

        public void Initialize()
        {
            View.OnNext += View_OnNext;
            Firmware = new CycleDictionary();
            Cpu = new ComputerUnit(this,View);
            Cpu.Initialize();
            CAR = new CyclePointer(0, Firmware.Cycle[OperationEnum.FETCH], true);
            CBR = new Instruction();
            InstructionDecoder = new Decoder(CBR, Cpu);
        }

        public void ReceiveInstructionRegister(string instructionRegister)
        {
            NextInstruction = new string(instructionRegister);
        }

        private void View_OnNext(object sender, EventArgs e)
        {
            if (ExternalMemory.EndOfProgram)
            {
                View.ShowMessage("Fim do Programa");
                return;
            }

            CheckForJumpFlags();
            GetNextFirmwareLine();
            InstructionDecoder.SendControlSignal(CBR);
            SendValuesToFront();
        }

        private void SendValuesToFront()
        {
            View.SetAC(Cpu.ACRegister.GetValue().GetIntString());
            View.SetIR(Cpu.InstructionRegister.GetValue());
            View.SetMAR(Cpu.MemoryAddressRegister.GetValue());
            View.SetPC(Cpu.ProgramCounter.GetValue().GetIntString());
            View.SetMBR(Cpu.MemoryBufferRegister.GetValue());
            View.SetX(Cpu.ULAXRegister.GetValue().GetIntString());
            View.SetS1(Cpu.S1Register.GetValue());
            View.SetS2(Cpu.S2Register.GetValue());
            View.SetS3(Cpu.S3Register.GetValue());
            View.SetS4(Cpu.S4Register.GetValue());
            View.SetSignalFlag(Cpu.ULA.SignalFlag.ToString());
            View.SetZeroFlag(Cpu.ULA.ZeroFlag.ToString());
        }

        private void CheckForJumpFlags()
        {
            if(!CBR.JumpIfZero && !CBR.JumpIfLessThan && !CBR.JumpIfDifferent)
            {
                CBR.ResetFlags();
                return;
            }

            if ( (CBR.JumpIfZero && Cpu.ULA.ZeroFlag) || 
                (CBR.JumpIfLessThan && Cpu.ULA.SignalFlag) || 
                (CBR.JumpIfDifferent && !Cpu.ULA.ZeroFlag) )
            {
                CBR.ResetFlags();
                return;
            }
            CBR.ResetFlags();
            GetNextCycle();

        }

        private void GetNextFirmwareLine()
        {
            if (CAR.IsEndOfCycle && NextInstruction != null)
            {
                GetNextCycle();
            }
            var nextInstruction = CAR.AccessNextCycleLine();
            CBR.SetControlSignalInstruction(nextInstruction);
        }

        private void GetNextCycle()
        {
            if (CAR.IsFetchCycle)
            {
                CAR.CurrentCycleEnum = OperationEnum.FETCH;
                CAR.CurrentCycle = Firmware.Cycle[OperationEnum.FETCH];
            }
            else
            {
                var op = DecodeOpCode();
                CAR.CurrentCycleEnum = op;
                CAR.CurrentCycle = Firmware.Cycle[op];
            }

            CAR.IsEndOfCycle = false;
            CAR.Clock = 0;
        }

        private OperationEnum DecodeOpCode()
        {
            var temp = NextInstruction.Substring(0, 6).ToCharArray();
            Array.Reverse(temp);
            BitArray opCode = new string(temp).GetBitArrayFromString();
            return (OperationEnum) opCode.GetIntFromBitArray();
        }

    }
}
