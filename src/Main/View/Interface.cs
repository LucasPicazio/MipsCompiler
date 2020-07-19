using Main.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Main
{
    public partial class Interface : Form
    {
        public Interface()
        {
            InitializeComponent();
            var uc = new UC();
            var compiler = new Compiler();

            uc.View = this;
            compiler.View = this;

            uc.Initialize();
            compiler.Initialize();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            this.button1.Click += OnPlay;
            this.button2.Click += OnNext;
        }

        public event EventHandler OnPlay;
        public event EventHandler OnNext;
        public List<Command> Assembly
        {
            get
            {
                var commands = new List<Command>();
                int antecessorLenght = 0;
                var lines = richTextBox1.Text.Split(Environment.NewLine);
                foreach (var line in lines)
                {
                    commands.Add(new Command
                    {
                        Text = line,
                        CharInit = antecessorLenght,
                        CharEnd = antecessorLenght + line.Length
                    });
                    antecessorLenght = line.Length;
                }
                return commands;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void HighLightLine(Command command)
        {
            richTextBox1.SelectionBackColor = Color.Yellow;
            if(command != null)
                richTextBox1.Select(command.CharInit, command.CharEnd);
        }

     
        public void SetIR(string text)
        {
            this.tbIR.Text = text;
        }
        public void SetMAR(string text)
        {
            this.tbMAR.Text = text;
        }
        public void SetPC(string text)
        {
            this.tbPC.Text = text;
        }
        public void SetMBR(string text)
        {
            this.tbMBR.Text = text;
        }
        public void SetS1(string text)
        {
            this.tbS1.Text = text;
        }
        public void SetS2(string text)
        {
            this.tbS2.Text = text;
        }
        public void SetS3(string text)
        {
            this.tbS3.Text = text;
        }
        public void SetS4(string text)
        {
            this.tbS4.Text = text;
        }
        public void SetX(string text)
        {
            this.tbX.Text = text;
        }
        public void SetAC(string text)
        {
            this.tbAc.Text = text;
        }

        public void SetZeroFlag(string text)
        {
            this.tbZeroFlag.Text = text;
        }
        public void SetSignalFlag(string text)
        {
            this.tbSignalFlag.Text = text;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        public void EnableNext()
        {
            button2.Enabled = true;
        }
    }
}
