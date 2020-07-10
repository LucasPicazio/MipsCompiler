using System;
using System.Collections;
using System.Collections.Generic;

namespace Main
{
    public class UC
    {
        public IForm View { get; internal set; }
        private ComputerUnit Cpu { get; set; }

        private string[] FetchCycle; 
        private string[] ExecuteCycle;
        private int Clock;

        internal void Initialize()
        {
            View.OnNext += View_OnNext;
            FetchCycle = new string[] 
            { 
            //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25|OP |R|W|J|JZ|J!|J>|J<|Address
                "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  0  001 0 0 0  0  0  0  0 000000".Replace(" ",""),
                "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  0  000 0 0 0  0  0  0  0 000000".Replace(" ",""),
                "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  1  000 1 0 0  0  0  0  0 000000".Replace(" ",""),
                "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0 000000".Replace(" ",""),
            };
            Cpu = new ComputerUnit(this);
            Cpu.Initialize();
        }

        private void View_OnNext(object sender, EventArgs e)
        {
            View.HighLightLine(new Model.Command { CharInit = 0, CharEnd = 8 });
            SendNextControlSignal(FetchCycle);
            Clock++;
        }

        public void ReceiveInstructionRegister(string instructionRegister)
        {
            throw new NotImplementedException();
        }

        private void SendNextControlSignal(string[] cycle)
        {
            Cpu.ReceiveControlSignal(cycle[Clock]);
        }
    }
}
