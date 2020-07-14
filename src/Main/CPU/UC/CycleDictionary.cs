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
                {OperationEnum.LI, LICycle },
                {OperationEnum.LW, LWCycle },
                {OperationEnum.LUI, LUICycle },
                {OperationEnum.ORI, ORICycle },
                {OperationEnum.SW, SWCycle },
                {OperationEnum.MOVE, MOVECycle },
                {OperationEnum.BEQ, BEQCycle },
                {OperationEnum.BNE, BNECycle },
                {OperationEnum.J, JCycle },
                {OperationEnum.SLT, SLTCycle },
                {OperationEnum.ADDI, ADDICycle },
            };
        }

        public Dictionary<OperationEnum, string[]> Cycle;

        private readonly string[] FetchCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  1  0  0  0  0  0  0  0  0  0  1  1  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  1  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] ADDCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };
        
        private readonly string[] SUBCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] LICycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] LWCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] LUICycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] ORICycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] SWCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] MOVECycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] BEQCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] BNECycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] JCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] SLTCycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };

        private readonly string[] ADDICycle = new string[]
        { 
        //  |0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24|OP |R|W|JZ|J!|J<|D1|D2|D3|
            "1 0 0 0 0 0 0 0 0 0 0  0  0  0  0  1  0  0  0  1  0  0  0  0  0  001 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 1 0 0 0 0 0 0 0 0 0  0  0  0  1  0  0  0  0  0  0  0  0  1  1  000 0 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 0 0 0 0 0 0 0 0 0  0  0  0  0  0  0  0  0  0  0  1  0  0  0  000 1 0 0  0  0  0  0  0".Replace(" ",""),
            "0 0 1 0 0 0 0 0 0 0 0  0  0  0  0  0  0  1  0  0  0  0  0  0  0  000 0 0 0  0  0  0  0  0".Replace(" ",""),
        };
    }
}
