using System.Collections.Generic;

namespace Main
{
    public class PortSignalMapping
    {

        // Identificador das operações para chamada dinâmica dos métodos
        public delegate void AccessPort();
        public Dictionary<int, AccessPort> delegateRegisterMapping = new Dictionary<int, AccessPort>();
        public readonly ComputerUnit Cpu;
        private readonly ExternalMemory Memory;
        public PortSignalMapping(ComputerUnit CPU)
        {
            Cpu = CPU;
            Memory = CPU.Memory;
        }

        public void Initialize()
        {
            delegateRegisterMapping = new Dictionary<int, AccessPort>()
            {
                #region Barramento Interno
                // Portas de leitura no barramento interno
                { 1, Cpu.ProgramCounter.GetValueFromIBus},
                { 3, Cpu.MemoryBufferRegister.GetValueFromIBus},
                { 5, Cpu.S1Register.GetValueFromIBus},
                { 7, Cpu.S2Register.GetValueFromIBus},
                { 9, Cpu.S3Register.GetValueFromIBus},
                { 11, Cpu.S4Register.GetValueFromIBus},
                { 13, Cpu.ULAXRegister.GetValueFromIBus},
                { 15, Cpu.ULA.GetValueFromIBus},
                { 17, Cpu.GetInstructionFromInternalBus},
                { 19, Cpu.MemoryAddressRegister.GetValueFromIBus},
                // Portas de escrita no barramento interno
                { 0, Cpu.ProgramCounter.SetValueIntoIBus},
                { 2, Cpu.MemoryBufferRegister.SetValueIntoIBus},
                { 4, Cpu.S1Register.SetValueIntoIBus},
                { 6, Cpu.S2Register.SetValueIntoIBus},
                { 8, Cpu.S3Register.SetValueIntoIBus},
                { 10, Cpu.S4Register.SetValueIntoIBus},
                { 12, Cpu.ACRegister.SetValueIntoIBus},
                { 14, Cpu.InstructionRegisterOpSource1.SetValueIntoIBus},
                { 16, Cpu.InstructionRegisterOpSource2.SetValueIntoIBus},
                { 18, Cpu.InstructionRegisterOpDestiny.SetValueIntoIBus},
                #endregion
                #region Barramento Externo
                // Portas de leitura do barramento externo
                { 21, Cpu.GetDataFromExternalBus},
                { 23, Memory.GetValueFromExternalBus},
                // Portas de escrita no barramento externo
                { 20, Cpu.SetDataIntoExternalBus},
                { 22, Cpu.SetAddressIntoExternalBus},
                { 24, Memory.SetDataIntoExternalBus}
                #endregion
            };
        }

    }
}
