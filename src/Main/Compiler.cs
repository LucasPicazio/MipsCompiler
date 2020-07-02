using Main.Model;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Main
{
    class Compiler
    {
        public IForm View { get; internal set; }

        internal void Initialize()
        {
            View.OnPlay += View_OnPlay;
        }

        private void View_OnPlay(object sender, EventArgs e)
        {
            var ass = View.Assembly;
            var result = MipsToBinary(ass);
        }

        private List<BitArray> MipsToBinary(List<Command> ass)
        {
            var result = new List<BitArray>();
            for (int i = 0; i < ass.Count; i++)
            {
                var parts = ass[i].Text.Split(' ').ToList();

                var errors = new List<string>();

                var opcodeString = parts[0];
                if (Enum.TryParse(opcodeString, true, out OperationEnum operation))
                    switch (operation)
                    {
                        case OperationEnum.ADD:
                        case OperationEnum.SUB:
                        case OperationEnum.SLT:
                            result.Add(RegisterFormat(operation, parts));
                            break;
                        case OperationEnum.J:
                            result.Add(JumpFormat(operation, parts));
                            break;
                        case OperationEnum.SW:
                        case OperationEnum.BEQ:
                        case OperationEnum.BNE:
                        case OperationEnum.ORI:
                        case OperationEnum.LW:
                        case OperationEnum.LUI:
                        case OperationEnum.ADDI:
                            result.Add(ImediateFormat(operation, parts));
                            break;
                        case OperationEnum.MOVE:
                        case OperationEnum.LI:
                            result.Add(PseudoFormat(operation, parts));
                            break;
                        default:
                            break;
                    }
                else
                    errors.Add($"Commando {opcodeString} não reconhecido na linha {i}");
            }

            return result;
        }

        private BitArray PseudoFormat(OperationEnum operation, List<string> parts)
        {
            var result = new BitArray(32);
            if (operation == OperationEnum.LI)
                result = LiOperation(parts);
            else
                result = MoveOperation(parts);

            return result;

        }

        private BitArray MoveOperation(List<string> parts)
        {
            return RegisterFormat(OperationEnum.ADD, new List<string> { "ADD", parts[1], "$zero", parts[2] });
        }

        private BitArray LiOperation(List<string> parts)
        {
            var result = new BitArray(32);
            if(Int16.TryParse(parts[2], out Int16 resultParse))// Menor que 16 bits
            {
                return RegisterFormat(OperationEnum.ADDI, new List<string> { "ADDI", parts[1], "$zero", resultParse.ToString() });
            }
            else if(Int32.TryParse(parts[2], out Int32 resultParse32))
            {
                var hexValue = resultParse32.ToString("X");
      
                if (hexValue.Count() % 2 != 0)// se for impar adiciona um 0
                    hexValue = "0" + hexValue;

                string part1 = hexValue.Substring(0, hexValue.Length / 2);
                string part2 = hexValue.Substring(hexValue.Length / 2, hexValue.Length / 2);

                result.Append(ImediateFormat(OperationEnum.LUI, new List<string> { "LUI", parts[1], part1 }));
                result.Append(ImediateFormat(OperationEnum.ORI, new List<string> { "ORI", parts[1], part2 }));

            }

            return result;

        }

        private BitArray RegisterFormat(OperationEnum operation, List<string> parts)
        {
            var result = new BitArray(32);
            result = result.Append(new BitArray(6,false));

            var register1String = parts[0].Trim(new char[] { '$',','});
            if (Enum.TryParse(register1String, true, out RegisterEnum register))
                result.Append(new BitArray((int)register));

            var register2String = parts[1].Trim(new char[] { '$', ',' });
            if (Enum.TryParse(register2String, true, out RegisterEnum register2))
                result.Append(new BitArray((int)register2));

            var register3String = parts[2].Trim(new char[] { '$', ',' });
            if (Enum.TryParse(register3String, true, out RegisterEnum register3))
                result.Append(new BitArray((int)register3));

            else if (Int16.TryParse(parts[2], out short resultParse)) // valor constante
                result.Append(new BitArray(resultParse));

            result = result.Append(new BitArray(5, false));

            result = result.Append(new BitArray((int)operation));

            return result;


        }

        private BitArray JumpFormat(OperationEnum operation, List<string> parts)
        {
            var result = new BitArray(32);
            result = result.Append(new BitArray((int)operation));
            if(Int32.TryParse(parts[1], out int refe))
                result = result.Append(new BitArray(refe));
            return result;

        }

        private BitArray ImediateFormat(OperationEnum operation, List<string> parts)
        {
            var result = new BitArray(32);
            result = result.Append(new BitArray((int)operation));

            var register1String = parts[1].Trim(new char[] { '$', ',' });
            if (Enum.TryParse(register1String, true, out RegisterEnum register))
                result.Append(new BitArray((int)register));

            var register2String = parts[2].Trim(new char[] { '$', ',' });
            if (Enum.TryParse(register2String, true, out RegisterEnum register2))
                result.Append(new BitArray((int)register2));

            if (Int32.TryParse(parts[3], out int refe))
                result = result.Append(new BitArray(refe));

            return result;
        }

       
    }
}
