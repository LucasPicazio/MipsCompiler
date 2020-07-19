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
        public IForm View { get; internal set; }
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
            Cpu = new ComputerUnit(this);
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
            View.HighLightLine(new Command { CharInit = 0, CharEnd = 8 });
            View.SetRegi1("asd");
            CheckForJumpFlags();
            GetNextFirmwareLine();
            InstructionDecoder.SendControlSignal(CBR);
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
