using System.Collections.Generic;

namespace Main
{
    public class PortSignalMapping
    {

        // Identificador das operações para chamada dinâmica dos métodos
        public delegate void AccessPort();
        public Dictionary<int, AccessPort> delegateRegisterMapping;
        private readonly ComputerUnit _CPU;
        private readonly ExternalMemory _Memory;
        public PortSignalMapping(ComputerUnit CPU)
        {
            _CPU = CPU;
            _Memory = new ExternalMemory();
        }

        public void Initialize()
        {
            delegateRegisterMapping = new Dictionary<int, AccessPort>()
            {
                #region Barramento Interno
                // Portas de leitura no barramento interno
                { 1, _CPU.ProgramCounter.GetValueFromIBus},
                { 3, _CPU.MemoryBufferRegister.GetValueFromIBus},
                { 5, _CPU.S1Register.GetValueFromIBus},
                { 7, _CPU.S2Register.GetValueFromIBus},
                { 9, _CPU.S3Register.GetValueFromIBus},
                { 11, _CPU.S4Register.GetValueFromIBus},
                { 13, _CPU.ULAXRegister.GetValueFromIBus},
                { 15, _CPU.ULA.GetValueFromIBus},
                { 17, _CPU.GetInstructionFromInternalBus},
                { 19, _CPU.MemoryAddressRegister.GetValueFromIBus},
                // Portas de escrita no barramento interno
                { 0, _CPU.ProgramCounter.SetValueIntoIBus},
                { 2, _CPU.MemoryBufferRegister.SetValueIntoIBus},
                { 4, _CPU.S1Register.SetValueIntoIBus},
                { 6, _CPU.S2Register.SetValueIntoIBus},
                { 8, _CPU.S3Register.SetValueIntoIBus},
                { 10, _CPU.S4Register.SetValueIntoIBus},
                { 12, _CPU.ACRegister.SetValueIntoIBus},
                { 14, _CPU.InstructionRegisterOpSource1.SetValueIntoIBus},
                { 16, _CPU.InstructionRegisterOpSource2.SetValueIntoIBus},
                { 18, _CPU.InstructionRegisterOpDestiny.SetValueIntoIBus},
                #endregion
                #region Barramento Externo
                // Portas de leitura do barramento externo
                { 21, _CPU.GetDataFromExternalBus},
                { 23, _Memory.GetValueFromExternalBus},
                // Portas de escrita no barramento externo
                { 20, _CPU.SetDataIntoExternalBus},
                { 22, _CPU.SetAddressIntoExternalBus},
                { 24, _Memory.SetDataIntoExternalBus},
                #endregion
            };
        }

    }
}
