using System;
using System.Collections.Generic;
using System.Text;

namespace Main.Model
{
    public enum OperationEnum
    {
        ADD = 32,
        SUB = 34,
        LI = 99, // pseudofunction
        LW = 35,
        LUI = 15,//necessary for li
        ORI = 13,//necessary for li 
        SW = 43,
        MOVE = 98, //pseudofunction
        BEQ = 5,
        BNE = 4,
        J = 2,
        SLT = 42,
        ADDI = 8,
        FETCH = 77
    }
}
