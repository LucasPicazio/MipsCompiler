using System;
using System.Collections.Generic;
using System.Text;

namespace Main
{
    class UC
    {
        public IForm View { get; internal set; }

        internal void Initialize()
        {
            View.OnNext += View_OnNext;
        }

        private void View_OnNext(object sender, EventArgs e)
        {
            View.HighLightLine(new Model.Command { CharInit = 0, CharEnd = 8 });
        }
    }
}
