namespace Main
{
    public class Instruction
    {
        public string ControlSignalInstruction { get; private set; }
        public bool JumpIfZero { get; set; }
        public bool JumpIfDifferent { get; set; }
        public bool JumpIfLessThan { get; set; }
        public bool DecodeRegisterSource1 { get; set; }
        public bool DecodeRegisterSource2 { get; set; }
        public bool DecodeRegisterDestiny { get; set; }

        public Instruction()
        {
            ResetFlags();
        }

        public void SetControlSignalInstruction(string value)
        {
            ControlSignalInstruction = new string(value);
            JumpIfZero = (ControlSignalInstruction[30] == '1');
            JumpIfDifferent = (ControlSignalInstruction[31] == '1');
            JumpIfLessThan = (ControlSignalInstruction[32] == '1');
            DecodeRegisterSource1 = (ControlSignalInstruction[33] == '1');
            DecodeRegisterSource2 = (ControlSignalInstruction[34] == '1');
            DecodeRegisterDestiny = (ControlSignalInstruction[35] == '1');
        }

        public void SetControlSignalInstruction(char[] value)
        {
            ControlSignalInstruction = new string(value);
            JumpIfZero = (ControlSignalInstruction[30] == '1');
            JumpIfDifferent = (ControlSignalInstruction[31] == '1');
            JumpIfLessThan = (ControlSignalInstruction[32] == '1');
            DecodeRegisterSource1 = (ControlSignalInstruction[33] == '1');
            DecodeRegisterSource2 = (ControlSignalInstruction[34] == '1');
            DecodeRegisterDestiny = (ControlSignalInstruction[35] == '1');
        }

        public void ResetFlags()
        {
            JumpIfZero = false;
            JumpIfDifferent = false;
            JumpIfLessThan = false;
            DecodeRegisterSource1 = false;
            DecodeRegisterSource2 = false;
            DecodeRegisterDestiny = false;
        }
    }
}