using Main.Model;
using System.Collections.Generic;

namespace Main.CPU.UC
{
    public class CycleDictionary
    {
        // Apenas o FetchCycle se encontra feito até o momento
        public CycleDictionary()
        {
            Cycle = new Dictionary<OperationEnum, string[]>()
            {
                {OperationEnum.FETCH, FetchCycle},
                {OperationEnum.ADD, ADDCycle },
                {OperationEnum.SUB, SUBCycle },
                {OperationEnum.SLT, SLTCycle },
                {OperationEnum.J, JCycle },
                {OperationEnum.SW, SWCycle },
                {OperationEnum.BEQ, BEQCycle },
                {OperationEnum.BNE, BNECycle },
                {OperationEnum.ORI, ORICycle },
                {OperationEnum.LW, LWCycle },
                {OperationEnum.LUI, LUICycle },
                {OperationEnum.ADDI, ADDICycle },
            };
        }

        public Dictionary<OperationEnum, string[]> Cycle;

        private readonly string[] FetchCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  1  0  0  0  0  0  0  0  0  0  1  1  0  000 0 0 0  0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  1  000 1 0 0  0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0  0".Replace(" ","")
        };

        #region Register Format
        private readonly string[] ADDCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // ADD IR1, IR2, IR3
            "0 0 0 0 0 0 0 0 0 0 0  0  0  1  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  1  0  0".Replace(" ",""), //        X <= REG(IR3) 
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  0  0  0  0  0  0  010 0 0 0  0  0  1  0  0  0".Replace(" ",""), // ULA(ADD) <= REG(IR2)
            "0 0 0 0 0 0 0 0 0 0 0  0  1  0  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  1  0".Replace(" ","")  // REG(IR1) <= AC
        };

        private readonly string[] SUBCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // SUB IR1, IR2, IR3
            "0 0 0 0 0 0 0 0 0 0 0  0  0  1  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  1  0  0".Replace(" ",""), //        X <= REG(IR3) 
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  0  0  0  0  0  0  011 0 0 0  0  0  1  0  0  0".Replace(" ",""), // ULA(SUB) <= REG(IR2)
            "0 0 0 0 0 0 0 0 0 0 0  0  1  0  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  1  0".Replace(" ","")  // REG(IR1) <= AC
        };

        private readonly string[] SLTCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // SLT IR1, IR2, IR3
            "0 0 0 0 0 0 0 0 0 0 0  0  0  1  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  1  0  0".Replace(" ",""), //  X <= REG(IR3)
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  0  0  0  0  0  0  100 0 0 0  0  0  1  0  0  0".Replace(" ",""), // ULA(CMP) <= REG(IR2) 
            "0 0 0 0 0 0 0 0 0 0 0  0  1  0  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  1  0".Replace(" ","")  // REG(IR1) <= AC
        };

        #endregion

        #region Jump Format
        private readonly string[] JCycle = new string[]
{ 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                // J Immediate
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  1  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0  0".Replace(" ","") // PC <= IR1
        };
        #endregion

        #region Immediate Format

        private readonly string[] SWCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // SW IR1, Immediate
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  1  0  0  0  0  0  000 0 0 0  0  0  0  0  0  0".Replace(" ",""), //      MAR <= IR2
            "0 0 0 1 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  0  1  1  0  000 0 0 0  0  0  0  0  1  0".Replace(" ",""), //      MEM <= MAR & MBR <= REG(IR1)
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  1  0  0  1  0  000 0 1 0  0  0  0  0  0  0".Replace(" ",""), // MEM(MAR) <= MBR
        };

        private readonly string[] BEQCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // BEQ IR1, IR2, Immediate
            "0 0 0 0 0 0 0 0 0 0 0  0  0  1  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  1  0  0  0".Replace(" ",""), //        X <= REG(IR2)
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  0  0  0  0  0  0  100 0 0 1  0  0  0  0  1  0".Replace(" ",""), // ULA(CMP) <= REG(IR1)
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  0  0  1  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0  0".Replace(" ",""), //       PC <= IR3
        };

        private readonly string[] BNECycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // BNE IR1, IR2, Immediate
            "0 0 0 0 0 0 0 0 0 0 0  0  0  1  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  1  0  0  0".Replace(" ",""), //        X <= REG(IR2) 
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  0  0  0  0  0  0  100 0 0 0  1  0  0  0  1  0".Replace(" ",""), // ULA(CMP) <= REG(IR1)
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  0  0  1  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0  0".Replace(" ",""), //       PC <= IR3
        };

        private readonly string[] ORICycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // ORI IR1, IR2, Immediate
            "0 0 0 0 0 0 0 0 0 0 0  0  0  1  0  0  1  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0  0".Replace(" ",""), //        X <= IR3
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  0  0  0  0  0  0  101 0 0 0  0  0  1  0  0  0".Replace(" ",""), //  ULA(OR) <= REG(IR2)
            "0 0 0 0 0 0 0 0 0 0 0  0  1  0  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  1  0".Replace(" ",""), // REG(IR1) <= AC
        };

        private readonly string[] LWCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // LW IR1, Immediate
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  1  0  0  0  0  0  000 0 0 0  0  0  0  0  0  0".Replace(" ",""), // MAR <= IR2
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  0  1  1  0  000 0 0 0  0  0  0  0  0  0".Replace(" ",""), // MEM <= MAR
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  1  000 1 0 0  0  0  0  0  0  0".Replace(" ",""), // MBR <= MEM(MAR)
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  1  0".Replace(" ","")  // REG(IR1) <= MBR
        };

        private readonly string[] LUICycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // LUI IR1, Immediate
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  1  0".Replace(" ","")  // REG(IR1) <= IR2
        };

        private readonly string[] ADDICycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D2|D3|D1|RZ|                 // ADDI IR1, IR2, Immediate
            "0 0 0 0 0 0 0 0 0 0 0  0  0  1  0  0  1  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0  0".Replace(" ",""), //        X <= IR3
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  0  0  0  0  0  0  010 0 0 0  0  0  1  0  0  0".Replace(" ",""), // ULA(ADD) <= REG(IR2)
            "0 0 0 0 0 0 0 0 0 0 0  0  1  0  0  0  0  0  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  1  0".Replace(" ","")  // REG(IR1) <= AC
        };
        #endregion
    }
}
