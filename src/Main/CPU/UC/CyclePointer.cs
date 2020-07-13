using System;
using System.Collections.Generic;
using System.Text;

namespace Main.CPU
{
    public class CyclePointer
    {
        public int Clock { get; set; }
        public string[] CurrentCycle { get; set; }
        public bool IsEndOfCycle { get; private set; }
        public bool IsFetchCycle { get; private set; }
        public CyclePointer(int address, string[] cycle)
        {
            this.Clock = address;
            this.CurrentCycle = cycle;
            IsFetchCycle = true;
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
