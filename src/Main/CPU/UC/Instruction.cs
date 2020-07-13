namespace Main
{
    public class Instruction
    {
        public string ControlSignalInstruction { get; private set; }
        public bool JumpIfZero { get; set; }
        public bool JumpIfDifferent { get; set; }
        public bool JumpIfLessThan { get; set; }

        public Instruction()
        {
            ResetJumpFlags();
        }

        public void SetControlSignalInstruction(string value)
        {
            ControlSignalInstruction = new string(value);
            JumpIfZero = (ControlSignalInstruction[31] == 1);
            JumpIfDifferent = (ControlSignalInstruction[32] == 1);
            JumpIfLessThan = (ControlSignalInstruction[33] == 1);
        }

        public void ResetJumpFlags()
        {
            JumpIfZero = false;
            JumpIfDifferent = false;
            JumpIfLessThan = false;
        }
    }
}