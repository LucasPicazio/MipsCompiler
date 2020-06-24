using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Main
{
    public static class Extensions
    {

        public static BitArray Append(this BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }
    }
}
