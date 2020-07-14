namespace Main
{
    public class Instruction
    {
        public string ControlSignalInstruction { get; private set; }
        public bool JumpIfZero { get; set; }
        public bool JumpIfDifferent { get; set; }
        public bool JumpIfLessThan { get; set; }
        public bool DecodeRegister1 { get; set; }
        public bool DecodeRegister2 { get; set; }
        public bool DecodeRegister3 { get; set; }

        public Instruction()
        {
            ResetFlags();
        }

        public void SetControlSignalInstruction(string value)
        {
            ControlSignalInstruction = new string(value);
            JumpIfZero = (ControlSignalInstruction[30] == 1);
            JumpIfDifferent = (ControlSignalInstruction[31] == 1);
            JumpIfLessThan = (ControlSignalInstruction[32] == 1);
            DecodeRegister1 = (ControlSignalInstruction[33] == 1);
            DecodeRegister2 = (ControlSignalInstruction[34] == 1);
            DecodeRegister3 = (ControlSignalInstruction[35] == 1);
        }

        public void ResetFlags()
        {
            JumpIfZero = false;
            JumpIfDifferent = false;
            JumpIfLessThan = false;
            DecodeRegister1 = false;
            DecodeRegister2 = false;
            DecodeRegister3 = false;
        }
    }
}