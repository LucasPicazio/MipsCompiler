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
        private string CBR;
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
            GetNextFirmwareLine();
            Cpu.ReceiveControlSignal(CBR);
        }

        private void GetNextFirmwareLine()
        {
            CBR = CAR.AccessNextCycleLine();
            if (CAR.IsEndOfCycle)
            {
                GetNextCycle();
            }
        }

        private void GetNextCycle()
        {
            if (CAR.IsFetchCycle)
            {
                CAR.CurrentCycle = Firmware.Cycle[OperationEnum.FETCH];
            }
            else
            {
                CAR.CurrentCycle = Firmware.Cycle[DecodeOpCode()];
            }
        }

        private OperationEnum DecodeOpCode()
        {
            BitArray opCode = NextInstruction.Substring(28).GetBitArrayFromString();
            return (OperationEnum) opCode.GetIntFromBitArray();
        }

        // SeqLogic > CAR (Address) > firmware > CBR (Command) > SeqLogic
    }
}
