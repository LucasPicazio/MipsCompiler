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
        private string NextInstruction;

        public void Initialize()
        {
            View.OnNext += View_OnNext;
            Firmware = new CycleDictionary();
            Cpu = new ComputerUnit(this);
            Cpu.Initialize();
            CAR = new CyclePointer(0, Firmware.Cycle[OperationEnum.FETCH], true);
            CBR = new Instruction();
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
            SendControlSignal();
        }

        private void SendControlSignal()
        {
            var instruct = CBR.ControlSignalInstruction;
            if (CBR.DecodeRegister1 || CBR.DecodeRegister2 || CBR.DecodeRegister3)
            {
                SetControlToInstructionFormat(instruct);
            }
            Cpu.ReceiveControlSignal(instruct);
        }

        private void SetControlToInstructionFormat(string instruct)
        {
            switch (Cpu.CurrentInstructFormat)
            {
                // Register Format
                case 1:
                    SetRegisterFormat(instruct);
                    break;
                // Imediate Format
                case 2:
                    SetImediateFormat(instruct);
                    break;
            }
        }

        private void SetRegisterFormat(string instruct)
        {
            throw new NotImplementedException();
        }

        private void SetImediateFormat(string instruct)
        {
            throw new NotImplementedException();
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
                CAR.CurrentCycleEnum = OperationEnum.FETCH;
                CAR.CurrentCycle = Firmware.Cycle[OperationEnum.FETCH];
            }
            else
            {
                var op = DecodeOpCode();
                CAR.CurrentCycleEnum = op;
                CAR.CurrentCycle = Firmware.Cycle[op];
            }

            CAR.Clock = 0;
            CBR.ResetFlags();
        }

        private OperationEnum DecodeOpCode()
        {
            BitArray opCode = NextInstruction.Substring(28).GetBitArrayFromString();
            return (OperationEnum) opCode.GetIntFromBitArray();
        }

    }
}
