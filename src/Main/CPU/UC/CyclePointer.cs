using Main.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main.CPU
{
    public class CyclePointer
    {
        public int Clock { get; set; }
        public string[] CurrentCycle { get; set; }
        public OperationEnum CurrentCycleEnum { get; set; }
        public bool IsEndOfCycle { get; set; }
        public bool IsFetchCycle { get; private set; }
        public CyclePointer(int address, string[] cycle, bool isFetchCycle)
        {
            this.Clock = address;
            this.CurrentCycle = cycle;
            IsFetchCycle = isFetchCycle;
        }

        
        public string AccessNextCycleLine()
        {
            var nextLine = CurrentCycle[Clock];
            Clock++;
            if (Clock >= CurrentCycle.Length)
            {
                IsEndOfCycle = true;
                IsFetchCycle = !IsFetchCycle;
            }
            return nextLine;
        }

    }
}
