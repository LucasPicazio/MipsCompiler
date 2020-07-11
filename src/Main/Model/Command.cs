using System.Collections;

namespace Main.Model
{
    public class Command
    {
        public string Text { get; set; }
        public BitArray Bits { get; set; }

        //metodos para fazer o HightLight
        public int CharInit { get; set; }
        public int CharEnd { get; set; }
    }
}
