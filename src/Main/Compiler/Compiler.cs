﻿using Main.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Main
{
    class Compiler
    {
        public Interface View { get; internal set; }
        public ExternalMemory Memory { get; internal set; }

        internal void Initialize()
        {
            View.OnPlay += View_OnPlay;
            Memory = new ExternalMemory();
        }

        private void View_OnPlay(object sender, EventArgs e)
        {
            var ass = View.Assembly;
            var result = MipsToBinary(ass);
            Memory.ReceiveAssemblyProgram(result);
            View.HighLightLine(ass[0]);
            View.EnableNext();
        }

        private List<Command> MipsToBinary(List<Command> ass)
        {
            var result = new List<Command>();
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
                            ass[i].Bits =  RegisterFormat(operation, parts);
                            result.Add(ass[i]);
                            break;
                        case OperationEnum.J:
                            ass[i].Bits =  JumpFormat(operation, parts);
                            result.Add(ass[i]);
                            break;
                        case OperationEnum.SW:
                        case OperationEnum.BEQ:
                        case OperationEnum.BNE:
                        case OperationEnum.ORI:
                        case OperationEnum.LW:
                        case OperationEnum.LUI:
                        case OperationEnum.ADDI:
                            ass[i].Bits =  ImediateFormat(operation, parts);
                            result.Add(ass[i]);
                            break;
                        case OperationEnum.MOVE:
                        case OperationEnum.LI:
                            foreach (var co in PseudoFormat(operation, parts))
                            {
                                ass[i].Bits = co;
                                result.Add(ass[i]);
                            }
                            break;
                        default:
                            break;
                    }
                else
                    errors.Add($"Commando {opcodeString} não reconhecido na linha {i}");
            }

            return result;
        }

        private List<BitArray> PseudoFormat(OperationEnum operation, List<string> parts)
        {
            var result = new List<BitArray>();
            if (operation == OperationEnum.LI)
                result = LiOperation(parts);
            else
                result =  MoveOperation(parts) ;

            return result;

        }

        private List<BitArray> MoveOperation(List<string> parts)
        {
            return new List<BitArray> { RegisterFormat(OperationEnum.ADD, new List<string> { "ADD", parts[1], "$zero", parts[2] }) };
        }

        private List<BitArray> LiOperation(List<string> parts)
        {
            var results = new List<BitArray>();
            if (Int16.TryParse(parts[2], out Int16 resultParse))// Menor que 16 bits
            {
                results.Add(RegisterFormat(OperationEnum.ADD, new List<string> { "ADD", parts[1], "$zero", resultParse.ToString() }));
            }
            else if (Int32.TryParse(parts[2], out Int32 resultParse32))
            {
                var hexValue = resultParse32.ToString("X");

                if (hexValue.Count() % 2 != 0)// se for impar adiciona um 0
                    hexValue = "0" + hexValue;

                string part1 = hexValue.Substring(0, hexValue.Length / 2);
                string part2 = hexValue.Substring(hexValue.Length / 2, hexValue.Length / 2);

                results.Add((ImediateFormat(OperationEnum.LUI, new List<string> { "LUI", parts[1], part1 })));
                results.Add((ImediateFormat(OperationEnum.ORI, new List<string> { "ORI", parts[1], part2 })));

            }

            return results;

        }

        private BitArray RegisterFormat(OperationEnum operation, List<string> parts)
        {
            var result = new BitArray(6, false);

            var register1String = parts[1].Trim(new char[] { '$', ',' });
            if (Enum.TryParse(register1String, true, out RegisterEnum register))
            {
                var bits = new BitArray(new byte[1] { (byte)register });
                bits = bits.Trim(5);

                result = result.Append(bits);
            }

            var register2String = parts[2].Trim(new char[] { '$', ',' });
            if (Enum.TryParse(register2String, true, out RegisterEnum register2))
            {
                var bits = new BitArray(new byte[1] { (byte)register2 });
                bits = bits.Trim(5);

                result = result.Append(bits);
            }

            var register3String = parts[3].Trim(new char[] { '$', ',' });
            if (Enum.TryParse(register3String, true, out RegisterEnum register3))
            {
                var bits = new BitArray(new byte[1] { (byte)register3 });
                bits = bits.Trim(5);

                result = result.Append(bits);
            }

            else if (Int16.TryParse(parts[3], out short resultParse)) // valor constante
                result.Append(new BitArray(new int[1] { resultParse }));

            result = result.Append(new BitArray(5, false));

            var bitsEX = new BitArray(new byte[1] { (byte)operation });
            bitsEX = bitsEX.Trim(6);

            result = result.Append(bitsEX);

            return result;


        }

        private BitArray JumpFormat(OperationEnum operation, List<string> parts)
        {
            var result = new BitArray(0);
            result = result.Append(new BitArray(new int[1] { (int)operation }).Trim(6));
            if(Int32.TryParse(parts[1], out int refe))
                result = result.Append(new BitArray(new int[1] { refe }).Trim(26));
            return result;

        }

        private BitArray ImediateFormat(OperationEnum operation, List<string> parts)
        {
            var result = new BitArray(0);
            result = result.Append(new BitArray(new int[1] { (int)operation }).Trim(6));

            var register1String = parts[1].Trim(new char[] { '$', ',' });
            if (Enum.TryParse(register1String, true, out RegisterEnum register))
                result =  result.Append(new BitArray(new int[1] { (int)register }).Trim(5));

            var register2String = parts[2].Trim(new char[] { '$', ',' });
            if (Enum.TryParse(register2String, true, out RegisterEnum register2))
                result = result.Append(new BitArray(new int[1] { (int)register2 }).Trim(5));

            if (Int32.TryParse(parts[3], out int refe))
                result = result.Append( new BitArray(new int[1] { refe }).Trim(16));

            return result;
        }

       
    }
}
