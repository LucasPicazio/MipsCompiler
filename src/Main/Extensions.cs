using System;
using System.Collections;
using System.Linq;

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

        public static int GetIntFromBitArray(this BitArray bitArray)
        {
            if (bitArray.Length > 32)
                throw new ArgumentException("Argumento não pode ter tamanho maior que 32");

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }

        public static BitArray GetBitArrayFromString(this string str)
        {
            return new BitArray(str.Select(c => c == '1').ToArray());
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
