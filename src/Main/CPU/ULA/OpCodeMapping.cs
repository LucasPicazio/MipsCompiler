using System;
using System.Collections.Generic;

namespace Main.ULA
{
    public class OpCodeMapping
    {
        public delegate void ExecuteOperation();
        public readonly Dictionary<string, ExecuteOperation> delegateOperationMapping;
        public readonly Dictionary<Tuple<int, int, int>, Tuple<int, int>> BitSumResultMap;
        private readonly ArithmeticLogicUnit _ULA;

        public OpCodeMapping(ArithmeticLogicUnit ULA)
        {
            _ULA = ULA;
            delegateOperationMapping = new Dictionary<string, ExecuteOperation>()
                {
                    { "001", _ULA.Increment},
                    { "010", _ULA.Sum},
                    { "011", _ULA.Subtract},
                    { "100", _ULA.Compare},
                    { "101", _ULA.OR}
                };

            BitSumResultMap = new Dictionary<Tuple<int, int, int>, Tuple<int, int>>()
            {
                { Tuple.Create(0,0,0), Tuple.Create(0,0) },
                { Tuple.Create(0,0,1), Tuple.Create(1,0) },
                { Tuple.Create(0,1,0), Tuple.Create(1,0) },
                { Tuple.Create(0,1,1), Tuple.Create(0,1) },
                { Tuple.Create(1,0,0), Tuple.Create(1,0) },
                { Tuple.Create(1,0,1), Tuple.Create(0,1) },
                { Tuple.Create(1,1,0), Tuple.Create(0,1) },
                { Tuple.Create(1,1,1), Tuple.Create(1,1) },
            };
        }

        

    }
}

