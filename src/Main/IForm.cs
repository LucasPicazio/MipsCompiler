using Main.Model;
using System;
using System.Collections.Generic;

namespace Main
{
    internal interface IForm
    {
        event EventHandler OnPlay;
        event EventHandler OnNext;
        List<Command> Assembly { get; }
        void HighLightLine(Command command);

        void SetRegi1(string text);
        void SetRegi2(string text);
        void SetRegi3(string text);
        void SetRegi4(string text);
        void ShowMessage(string message);
    }
}