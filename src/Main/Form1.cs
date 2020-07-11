using Main.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class Form1 :Form, IForm
    {
        public event EventHandler OnPlay;
        public event EventHandler OnNext;
        public List<Command> Assembly
        {
            get
            {
                var commands = new List<Command>();
                int antecessorLenght = 0;
                var lines  = richTextBox1.Text.Split(Environment.NewLine);
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

        public Form1()
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

        public void HighLightLine(Command command)
        {
            richTextBox1.SelectionColor = Color.Red;
            richTextBox1.Select(command.CharInit, command.CharEnd);
        }

        public void SetRegi1(string text)
        {
            tbRegi1.Text = text;
        }

        public void SetRegi2(string text)
        {
            tbRegi2.Text = text;
        }

        public void SetRegi3(string text)
        {
            tbRegi3.Text = text;
        }

        public void SetRegi4(string text)
        {
            tbRegi4.Text = text;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

    }
}
