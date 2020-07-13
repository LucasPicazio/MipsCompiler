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
        public IForm View { get; internal set; }
        private ComputerUnit Cpu { get; set; }
        private CycleDictionary Firmware;
        private CyclePointer CAR;
        private Instruction CBR;
        private string NextInstruction;

        public void Initialize()
        {
            View.OnNext += View_OnNext;
            Firmware = new CycleDictionary();
            Cpu = new ComputerUnit(this);
            Cpu.Initialize();
            CAR = new CyclePointer(0, Firmware.Cycle[OperationEnum.FETCH]);
        }

        public void ReceiveInstructionRegister(string instructionRegister)
        {
            NextInstruction = new string(instructionRegister);
        }

        private void View_OnNext(object sender, EventArgs e)
        {
            View.HighLightLine(new Command { CharInit = 0, CharEnd = 8 });
            CheckForJumpFlags();
            GetNextFirmwareLine();
            Cpu.ReceiveControlSignal(CBR.ControlSignalInstruction);
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
                return;
            }

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
                CAR.CurrentCycle = Firmware.Cycle[OperationEnum.FETCH];
            }
            else
            {
                CAR.CurrentCycle = Firmware.Cycle[DecodeOpCode()];
            }
            CAR.Clock = 0;
            CBR.ResetJumpFlags();
        }

        private OperationEnum DecodeOpCode()
        {
            BitArray opCode = NextInstruction.Substring(28).GetBitArrayFromString();
            return (OperationEnum) opCode.GetIntFromBitArray();
        }

        // SeqLogic > CAR (Address) > firmware > CBR (Command) > SeqLogic
    }
}
