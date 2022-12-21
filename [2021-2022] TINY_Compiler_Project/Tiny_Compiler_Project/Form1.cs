using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            string Code = textBox1.Text;
            Tiny_Compiler_Project.Start_Compiling(Code);
            PrintTokens();
            PrintErrors();
        }
        void PrintTokens()
        {
            for (int i = 0; i < Tiny_Compiler_Project.Tiny_Scanner.Tokens.Count; i++)
            {
                dataGridView1.Rows.Add(Tiny_Compiler_Project.Tiny_Scanner.Tokens.ElementAt(i).lex, Tiny_Compiler_Project.Tiny_Scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
            {
                 textBox2.Text += Errors.Error_List[i];
                textBox2.Text += "\r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            textBox2.Text = "";
            Errors.Error_List.Clear();
            Tiny_Compiler_Project.TokenStream.Clear();
        }
    }
}
