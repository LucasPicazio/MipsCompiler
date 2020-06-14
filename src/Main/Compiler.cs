using System;
using System.Collections.Generic;
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
            Console.WriteLine(ass);
        }
    }
}
