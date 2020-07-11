using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Main
{
    public static class Extensions
    {
        ///<summary>
        /// Append one bitArray to another
        ///</summary>
        public static BitArray Append(this BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }


        ///<summary>
        /// Trim BitArray to specified length
        ///</summary>
        public static BitArray Trim(this BitArray current, int length)
        {
            try
            {
                var bools = new bool[length];
                for (int i = 0; i < length; i++)
                {
                    bools[i] = current[i];
                }

                return new BitArray(bools);
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException($"Erro no Trim. Valor maior que {length} bits");
            }
            
        }
    }
}
