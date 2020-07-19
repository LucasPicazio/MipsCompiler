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
            CheckForJumpFlags();
            GetNextFirmwareLine();
            InstructionDecoder.SendControlSignal(CBR);
            SendValuesToFront();
        }

        private void SendValuesToFront()
        {
            View.SetAC(Cpu.ACRegister.GetValue().GetIntString());
            View.SetIR(Cpu.InstructionRegister.GetValue().GetIntString());
            View.SetMAR(Cpu.MemoryAddressRegister.GetValue().GetIntString());
            View.SetPC(Cpu.ProgramCounter.GetValue().GetIntString());
            View.SetMBR(Cpu.MemoryBufferRegister.GetValue().GetIntString());
            View.SetX(Cpu.ULAXRegister.GetValue().GetIntString());
            View.SetS1(Cpu.S1Register.GetValue().GetIntString());
            View.SetS2(Cpu.S2Register.GetValue().GetIntString());
            View.SetS3(Cpu.S3Register.GetValue().GetIntString());
            View.SetS4(Cpu.S4Register.GetValue().GetIntString());
            View.SetSignalFlag(Cpu.ULA.SignalFlag.ToString());
            View.SetZeroFlag(Cpu.ULA.ZeroFlag.ToString());
        }

        private void CheckForJumpFlags()
        {
            if(!CBR.JumpIfZero && !CBR.JumpIfLessThan && !CBR.JumpIfDifferent)
            {
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
            var nextInstruction = CAR.AccessNextCycleLine();
            CBR.SetControlSignalInstruction(nextInstruction);
            if (CAR.IsEndOfCycle)
            {
                GetNextCycle();
            }
        }

        private void GetNextCycle()
        {
            if (!CAR.IsFetchCycle)
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
            BitArray opCode = NextInstruction.Substring(25, 3).GetBitArrayFromString();
            return (OperationEnum) opCode.GetIntFromBitArray();
        }

    }
}
